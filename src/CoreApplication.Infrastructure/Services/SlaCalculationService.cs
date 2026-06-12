using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Domain.Entities.Sla;
using CoreApplication.Domain.Enums;
using CoreApplication.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CoreApplication.Infrastructure.Services;

public class SlaCalculationService(
    ApplicationDbContext context,
    IDateTimeService dateTime,
    ILogger<SlaCalculationService> logger)
    : ISlaCalculationService
{
    public async Task RecalculateAllAsync(CancellationToken cancellationToken = default)
    {
        var config = await LoadGlobalConfigAsync(cancellationToken);
        if (config is null)
        {
            logger.LogWarning("No SlaGlobalConfig found. Skipping SLA calculation.");
            return;
        }

        var courierIds = await context.Couriers
            .Where(c => c.IsActive && !c.IsDeleted)
            .Select(c => c.Id)
            .ToListAsync(cancellationToken);

        var today = DateOnly.FromDateTime(dateTime.UtcNow);

        foreach (var courierId in courierIds)
        {
            try
            {
                var slaConfigs = await context.CourierSlaConfigs
                    .Where(s => s.CourierId == courierId && s.IsActive && !s.IsDeleted)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                await CalculateAndUpsertAsync(courierId, SlaPeriodType.Rolling30, GetRolling30Period(today), config, slaConfigs, cancellationToken);
                await CalculateAndUpsertAsync(courierId, SlaPeriodType.Daily, GetDailyPeriod(today), config, slaConfigs, cancellationToken);
                await CalculateAndUpsertAsync(courierId, SlaPeriodType.Weekly, GetWeeklyPeriod(today), config, slaConfigs, cancellationToken);
                await CalculateAndUpsertAsync(courierId, SlaPeriodType.Monthly, GetMonthlyPeriod(today), config, slaConfigs, cancellationToken);

                await context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed SLA calculation for courier {CourierId}", courierId);
            }
        }

        logger.LogInformation("SLA recalculation complete for {Count} couriers", courierIds.Count);
    }

    // ── Core calculation ──────────────────────────────────────────────────────

    private async Task CalculateAndUpsertAsync(
        int courierId,
        SlaPeriodType periodType,
        (DateOnly Start, DateOnly End) period,
        SlaGlobalConfig config,
        List<Domain.Entities.Courier.CourierSlaConfig> slaConfigs,
        CancellationToken cancellationToken)
    {
        var periodStartDt = period.Start.ToDateTime(TimeOnly.MinValue);
        var periodEndDt = period.End.ToDateTime(TimeOnly.MaxValue);

        var rawStats = await context.ParcelCourierFleets
            .Where(pcf =>
                pcf.ParcelCourier.CourierId == courierId &&
                pcf.CreatedAt >= periodStartDt &&
                pcf.CreatedAt <= periodEndDt)
            .Select(pcf => new
            {
                pcf.Status,
                pcf.DeliveredAt,
                pcf.ParcelCourier.PromisedDeliveryAt,
                pcf.ParcelCourier.AssignedAt,
                Cost = (decimal?)pcf.ParcelCourier.Cost!.DistributionCost
            })
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var totalAssigned = rawStats.Count;
        var hasMissingKpi = totalAssigned == 0;

        var totalDelivered = rawStats.Count(s => s.Status == ParcelTrackingEnum.FinalizedDelivery);
        var totalOnTime = rawStats.Count(s =>
            s.Status == ParcelTrackingEnum.FinalizedDelivery &&
            s.DeliveredAt.HasValue &&
            s.DeliveredAt.Value <= s.PromisedDeliveryAt);
        var totalReturned = rawStats.Count(s => s.Status == ParcelTrackingEnum.Returned);

        var deliveredItems = rawStats
            .Where(s => s.Status == ParcelTrackingEnum.FinalizedDelivery && s.DeliveredAt.HasValue)
            .ToList();

        decimal? avgDeliveryHours = deliveredItems.Count > 0
            ? (decimal)deliveredItems.Average(s => (s.DeliveredAt!.Value - s.AssignedAt).TotalHours)
            : null;

        decimal? avgCostPerParcel = rawStats.Any(s => s.Cost.HasValue)
            ? rawStats.Where(s => s.Cost.HasValue).Average(s => s.Cost!.Value)
            : null;

        // Rates
        var successRate = totalAssigned > 0 ? (decimal)totalDelivered / totalAssigned : 0m;
        var onTimeRate = totalDelivered > 0 ? (decimal)totalOnTime / totalDelivered : 0m;
        var returnRate = totalAssigned > 0 ? (decimal)totalReturned / totalAssigned : 0m;

        // Component scores (0..1)
        var slaConfig = slaConfigs.FirstOrDefault(s => s.ServiceType == ServiceType.LastMile);

        decimal? deliveryTimeScore = null;
        if (avgDeliveryHours.HasValue && slaConfig != null && slaConfig.SlaHours > 0)
            deliveryTimeScore = Math.Min(1m, slaConfig.SlaHours / avgDeliveryHours.Value);

        decimal? costEfficiencyScore = null;
        if (avgCostPerParcel.HasValue && avgCostPerParcel.Value > 0 && config.BenchmarkCostPerParcel > 0)
            costEfficiencyScore = Math.Min(1m, config.BenchmarkCostPerParcel / avgCostPerParcel.Value);

        // Weighted SLA score
        var slaScore =
            successRate * config.WeightSuccessRate +
            onTimeRate * config.WeightOnTimeDelivery +
            (1m - returnRate) * config.WeightReturnRate +
            (deliveryTimeScore ?? 0m) * config.WeightDeliveryTime +
            (costEfficiencyScore ?? 0m) * config.WeightCost;

        // Missing KPI flags bitmask
        var missingKpiFlags = 0;
        if (totalAssigned == 0) missingKpiFlags |= 1;
        if (!avgDeliveryHours.HasValue) missingKpiFlags |= 2;
        if (!avgCostPerParcel.HasValue) missingKpiFlags |= 4;

        // Upsert snapshot
        var snapshot = await context.CarrierSlaSnapshots
            .FirstOrDefaultAsync(s =>
                s.CourierId == courierId &&
                s.PeriodType == periodType &&
                s.PeriodStart == period.Start,
                cancellationToken);

        var now = dateTime.UtcNow;

        if (snapshot is null)
        {
            snapshot = new CarrierSlaSnapshot
            {
                CourierId = courierId,
                SlaGlobalConfigId = config.Id,
                PeriodType = periodType,
                PeriodStart = period.Start,
                PeriodEnd = period.End,
                CreatedAt = now
            };
            context.CarrierSlaSnapshots.Add(snapshot);
        }

        snapshot.TotalAssigned = totalAssigned;
        snapshot.TotalDelivered = totalDelivered;
        snapshot.TotalOnTime = totalOnTime;
        snapshot.TotalReturned = totalReturned;
        snapshot.AvgDeliveryHours = avgDeliveryHours;
        snapshot.AvgCostPerParcel = avgCostPerParcel;
        snapshot.SuccessRate = successRate;
        snapshot.OnTimeDelivery = onTimeRate;
        snapshot.ReturnRate = returnRate;
        snapshot.DeliveryTimeScore = deliveryTimeScore;
        snapshot.CostEfficiencyScore = costEfficiencyScore;
        snapshot.SlaScore = slaScore;
        snapshot.HasMissingKpi = hasMissingKpi;
        snapshot.MissingKpiFlags = missingKpiFlags;

        // Save first to get snapshot.Id for alerts
        await context.SaveChangesAsync(cancellationToken);

        await GenerateAlertsAsync(snapshot, slaConfig, cancellationToken);
    }

    // ── Alert generation ──────────────────────────────────────────────────────

    private async Task GenerateAlertsAsync(
        CarrierSlaSnapshot snapshot,
        Domain.Entities.Courier.CourierSlaConfig? slaConfig,
        CancellationToken cancellationToken)
    {
        var existingAlertTypes = await context.SlaAlerts
            .Where(a => a.SnapshotId == snapshot.Id && !a.IsRead)
            .Select(a => a.AlertType)
            .ToListAsync(cancellationToken);

        void AddAlert(SlaAlertType alertType)
        {
            if (existingAlertTypes.Contains(alertType)) return;
            context.SlaAlerts.Add(new SlaAlert
            {
                CourierId = snapshot.CourierId,
                SnapshotId = snapshot.Id,
                AlertType = alertType,
                IsRead = false,
                CreatedAt = dateTime.UtcNow
            });
        }

        if (snapshot.HasMissingKpi)
            AddAlert(SlaAlertType.MissingKpiData);

        if (snapshot.SlaScore < 0.70m)
            AddAlert(SlaAlertType.SlaScoreLow);

        if (slaConfig is not null)
        {
            if (snapshot.SuccessRate < slaConfig.SuccessRateMin)
                AddAlert(SlaAlertType.SuccessRateLow);

            if (snapshot.OnTimeDelivery < slaConfig.OnTimeMin)
                AddAlert(SlaAlertType.OnTimeDeliveryLow);

            if (snapshot.ReturnRate > slaConfig.ReturnRateMax)
                AddAlert(SlaAlertType.ReturnRateHigh);
        }

        if (snapshot.DeliveryTimeScore.HasValue && snapshot.DeliveryTimeScore.Value < 0.80m)
            AddAlert(SlaAlertType.DeliveryTimeSlow);

        if (snapshot.CostEfficiencyScore.HasValue && snapshot.CostEfficiencyScore.Value < 0.80m)
            AddAlert(SlaAlertType.CostHigh);
    }

    // ── Period helpers ────────────────────────────────────────────────────────

    private static (DateOnly Start, DateOnly End) GetRolling30Period(DateOnly today)
        => (today.AddDays(-30), today);

    private static (DateOnly Start, DateOnly End) GetDailyPeriod(DateOnly today)
        => (today.AddDays(-1), today.AddDays(-1));

    private static (DateOnly Start, DateOnly End) GetWeeklyPeriod(DateOnly today)
    {
        var daysFromMonday = ((int)today.DayOfWeek + 6) % 7;
        var lastMonday = today.AddDays(-daysFromMonday - 7);
        return (lastMonday, lastMonday.AddDays(6));
    }

    private static (DateOnly Start, DateOnly End) GetMonthlyPeriod(DateOnly today)
    {
        var firstOfLastMonth = new DateOnly(today.Year, today.Month, 1).AddMonths(-1);
        var lastOfLastMonth = new DateOnly(today.Year, today.Month, 1).AddDays(-1);
        return (firstOfLastMonth, lastOfLastMonth);
    }

    private async Task<SlaGlobalConfig?> LoadGlobalConfigAsync(CancellationToken cancellationToken)
        => await context.SlaGlobalConfigs
            .OrderByDescending(c => c.EffectiveFrom)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
}

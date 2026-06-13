using CoreApplication.Domain.Enums;

namespace CoreApplication.Application.Features.Sla.Common;

public record SlaSnapshotDto(
    int Id,
    int CourierId,
    string CourierTitle,
    SlaPeriodType PeriodType,
    DateOnly PeriodStart,
    DateOnly PeriodEnd,
    int TotalAssigned,
    int TotalDelivered,
    int TotalOnTime,
    int TotalReturned,
    decimal? AvgDeliveryHours,
    decimal? AvgCostPerParcel,
    decimal SuccessRate,
    decimal OnTimeDelivery,
    decimal ReturnRate,
    decimal? DeliveryTimeScore,
    decimal? CostEfficiencyScore,
    decimal SlaScore,
    bool HasMissingKpi,
    DateTime CreatedAt);

using CoreApplication.Application.Common.Exceptions;
using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Application.Features.CourierPanel.Common;
using CoreApplication.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.CourierPanel.Queries.GetFleetDailyReport;

public class GetFleetDailyReportQueryHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser)
    : IRequestHandler<GetFleetDailyReportQuery, List<FleetDailyReportDto>>
{
    public async Task<List<FleetDailyReportDto>> Handle(
        GetFleetDailyReportQuery request,
        CancellationToken cancellationToken)
    {
        var courierId = currentUser.CourierId ?? throw new ForbiddenAccessException();
        var date = request.Date ?? DateOnly.FromDateTime(DateTime.Today);
        var dayStart = date.ToDateTime(TimeOnly.MinValue);
        var dayEnd = date.ToDateTime(TimeOnly.MaxValue);

        // Active fleets for this courier
        var fleets = await context.Fleets
            .Where(f => f.CourierId == courierId && f.IsActive && !f.IsDeleted)
            .Select(f => new
            {
                f.Id,
                f.Plaque,
                DriverFirstName = f.UserAccount.FirstName,
                DriverLastName = f.UserAccount.LastName
            })
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        if (fleets.Count == 0) return [];

        var fleetIds = fleets.Select(f => f.Id).ToList();

        // All parcel fleet records for these fleets on this date (non-cancelled)
        var fleetStats = await context.ParcelCourierFleets
            .Where(pf =>
                fleetIds.Contains(pf.FleetId) &&
                !pf.IsDeleted &&
                pf.Status != ParcelTrackingEnum.AssignCancelled &&
                pf.CreatedAt >= dayStart && pf.CreatedAt <= dayEnd)
            .GroupBy(pf => pf.FleetId)
            .Select(g => new
            {
                FleetId = g.Key,
                Total = g.Count(),
                Delivered = g.Count(pf => pf.Status == ParcelTrackingEnum.FinalizedDelivery),
                Returned = g.Count(pf => pf.Status == ParcelTrackingEnum.Returned),
                Pending = g.Count(pf =>
                    pf.Status == ParcelTrackingEnum.AssignToFleet ||
                    pf.Status == ParcelTrackingEnum.ReceivedByFleet),
                Unsettled = g.Count(pf =>
                    !pf.IsDailySettlement &&
                    (pf.Status == ParcelTrackingEnum.FinalizedDelivery ||
                     pf.Status == ParcelTrackingEnum.Returned))
            })
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        // COD check: any parcel for these fleets on this date has COD
        var codFleetIds = await context.ParcelCourierFleets
            .Where(pf =>
                fleetIds.Contains(pf.FleetId) &&
                !pf.IsDeleted &&
                pf.Status != ParcelTrackingEnum.AssignCancelled &&
                pf.CreatedAt >= dayStart && pf.CreatedAt <= dayEnd &&
                pf.ParcelCourier.Parcel.HasCOD)
            .Select(pf => pf.FleetId)
            .Distinct()
            .ToListAsync(cancellationToken);

        var statsMap = fleetStats.ToDictionary(s => s.FleetId);
        var codSet = codFleetIds.ToHashSet();

        return fleets.Select(f =>
        {
            var s = statsMap.GetValueOrDefault(f.Id);
            return new FleetDailyReportDto(
                f.Id,
                f.Plaque,
                f.DriverFirstName ?? "",
                f.DriverLastName ?? "",
                s?.Total ?? 0,
                s?.Delivered ?? 0,
                s?.Returned ?? 0,
                s?.Pending ?? 0,
                s?.Unsettled ?? 0,
                codSet.Contains(f.Id));
        }).ToList();
    }
}

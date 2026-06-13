using CoreApplication.Application.Common.Exceptions;
using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Application.Features.CourierPanel.Common;
using CoreApplication.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.CourierPanel.Queries.GetDriverSettlementReport;

public class GetDriverSettlementReportQueryHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser)
    : IRequestHandler<GetDriverSettlementReportQuery, List<DriverSettlementReportDto>>
{
    public async Task<List<DriverSettlementReportDto>> Handle(
        GetDriverSettlementReportQuery request,
        CancellationToken cancellationToken)
    {
        var courierId = currentUser.CourierId ?? throw new ForbiddenAccessException();
        var dayStart = request.Date.ToDateTime(TimeOnly.MinValue);
        var dayEnd = request.Date.ToDateTime(TimeOnly.MaxValue);

        // Load all fleet assignment records for this courier on this day (non-cancelled)
        var records = await context.ParcelCourierFleets
            .Where(f =>
                f.Fleet.CourierId == courierId &&
                f.Status != ParcelTrackingEnum.AssignCancelled &&
                !f.IsDeleted &&
                f.CreatedAt >= dayStart && f.CreatedAt <= dayEnd)
            .Select(f => new
            {
                f.FleetId,
                Plaque = f.Fleet.Plaque,
                DriverFirstName = f.Fleet.UserAccount.FirstName,
                DriverLastName = f.Fleet.UserAccount.LastName,
                f.Status,
                f.IsDailySettlement,
                HasCOD = f.ParcelCourier.Parcel.HasCOD,
                ParcelValue = f.ParcelCourier.Parcel.Value
            })
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        if (records.Count == 0) return [];

        return records
            .GroupBy(r => new { r.FleetId, r.Plaque, r.DriverFirstName, r.DriverLastName })
            .Select(g =>
            {
                var all = g.ToList();
                var total = all.Count;
                var delivered = all.Count(r => r.Status == ParcelTrackingEnum.FinalizedDelivery);
                var returned = all.Count(r => r.Status == ParcelTrackingEnum.Returned);
                var inProgress = all.Count(r => r.Status == ParcelTrackingEnum.ReceivedByFleet);

                var finalized = delivered + returned;
                var progress = total > 0
                    ? Math.Round((decimal)finalized / total * 100, 1)
                    : 0;

                var totalCod = all
                    .Where(r => r.HasCOD)
                    .Sum(r => r.ParcelValue);

                var deliveredCod = all
                    .Where(r => r.Status == ParcelTrackingEnum.FinalizedDelivery && r.HasCOD)
                    .Sum(r => r.ParcelValue);

                var settledCount = all.Count(r =>
                    r.IsDailySettlement &&
                    (r.Status == ParcelTrackingEnum.FinalizedDelivery ||
                     r.Status == ParcelTrackingEnum.Returned));

                var isFullySettled = finalized > 0 && settledCount == finalized;

                return new DriverSettlementReportDto(
                    g.Key.FleetId,
                    g.Key.Plaque,
                    g.Key.DriverFirstName ?? "",
                    g.Key.DriverLastName ?? "",
                    total,
                    delivered,
                    returned,
                    inProgress,
                    progress,
                    totalCod,
                    deliveredCod,
                    settledCount,
                    isFullySettled);
            })
            .OrderBy(r => r.DriverFirstName)
            .ToList();
    }
}

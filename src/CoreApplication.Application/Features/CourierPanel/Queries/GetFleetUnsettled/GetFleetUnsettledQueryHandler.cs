using CoreApplication.Application.Common.Exceptions;
using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Application.Features.CourierPanel.Common;
using CoreApplication.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.CourierPanel.Queries.GetFleetUnsettled;

public class GetFleetUnsettledQueryHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser)
    : IRequestHandler<GetFleetUnsettledQuery, List<UnsettledParcelDto>>
{
    public async Task<List<UnsettledParcelDto>> Handle(
        GetFleetUnsettledQuery request,
        CancellationToken cancellationToken)
    {
        var courierId = currentUser.CourierId ?? throw new ForbiddenAccessException();

        // Security: ensure fleet belongs to this courier
        var fleetBelongs = await context.Fleets
            .AnyAsync(f => f.Id == request.FleetId && f.CourierId == courierId && !f.IsDeleted,
                cancellationToken);

        if (!fleetBelongs) throw new ForbiddenAccessException();

        var query = context.ParcelCourierFleets
            .Where(pf =>
                pf.FleetId == request.FleetId &&
                !pf.IsDailySettlement &&
                (pf.Status == ParcelTrackingEnum.FinalizedDelivery ||
                 pf.Status == ParcelTrackingEnum.Returned) &&
                !pf.IsDeleted);

        if (request.Date.HasValue)
        {
            var dayStart = request.Date.Value.ToDateTime(TimeOnly.MinValue);
            var dayEnd = request.Date.Value.ToDateTime(TimeOnly.MaxValue);
            query = query.Where(pf => pf.CreatedAt >= dayStart && pf.CreatedAt <= dayEnd);
        }

        return await query
            .Select(pf => new UnsettledParcelDto(
                pf.Id,
                pf.ParcelCourierId,
                pf.ParcelCourier.Parcel.Barcode,
                pf.Status,
                pf.ParcelCourier.DistributeFirstName,
                pf.ParcelCourier.DistributeLastName,
                pf.ParcelCourier.DistributeAddress,
                pf.ParcelCourier.Parcel.HasCOD,
                pf.ParcelCourier.Parcel.Value,
                pf.DeliveredAt,
                pf.ReturnReason,
                pf.ReturnNote))
            .AsNoTracking()
            .OrderBy(pf => pf.Status)
            .ToListAsync(cancellationToken);
    }
}

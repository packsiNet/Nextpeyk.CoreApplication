using CoreApplication.Application.Common.Exceptions;
using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Application.Features.Parcels.Common;
using CoreApplication.Domain.Entities.Shipment;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.Parcels.Queries.TrackParcel;

public class TrackParcelQueryHandler(IApplicationDbContext context, ISenderContext senderContext)
    : IRequestHandler<TrackParcelQuery, ParcelTrackDto>
{
    public async Task<ParcelTrackDto> Handle(TrackParcelQuery request, CancellationToken cancellationToken)
    {
        var parcel = await context.Parcels
            .Include(p => p.ParcelCouriers.Where(pc => !pc.IsDeleted))
                .ThenInclude(pc => pc.Courier)
            .Include(p => p.ParcelCouriers.Where(pc => !pc.IsDeleted))
                .ThenInclude(pc => pc.ParcelCourierFleets.Where(f => !f.IsDeleted))
            .Where(p => p.Barcode == request.Barcode &&
                        p.SenderId == senderContext.SenderId &&
                        !p.IsDeleted)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException(nameof(Parcel), request.Barcode);

        // آخرین leg فعال
        var activeParcelCourier = parcel.ParcelCouriers
            .OrderByDescending(pc => pc.AssignedAt)
            .FirstOrDefault();

        // آخرین وضعیت fleet برای این parcel courier
        var latestFleet = activeParcelCourier?.ParcelCourierFleets
            .OrderByDescending(f => f.CreatedAt)
            .FirstOrDefault();

        return new ParcelTrackDto(
            parcel.Id,
            parcel.Barcode,
            parcel.State,
            activeParcelCourier?.Courier.Title,
            activeParcelCourier?.Status,
            activeParcelCourier?.AssignedAt ?? parcel.CreatedAt,
            activeParcelCourier?.PromisedDeliveryAt,
            latestFleet?.DeliveredAt,
            latestFleet?.ReturnReason,
            latestFleet?.ReturnNote);
    }
}

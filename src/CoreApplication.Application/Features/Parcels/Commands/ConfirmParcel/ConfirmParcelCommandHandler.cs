using CoreApplication.Application.Common.Exceptions;
using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Domain.Entities.Shipment;
using CoreApplication.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace CoreApplication.Application.Features.Parcels.Commands.ConfirmParcel;

public class ConfirmParcelCommandHandler(
    IApplicationDbContext context,
    ISenderContext senderContext,
    ICurrentUserService currentUser,
    IDateTimeService dateTime)
    : IRequestHandler<ConfirmParcelCommand>
{
    public async Task Handle(ConfirmParcelCommand request, CancellationToken cancellationToken)
    {
        var parcel = await context.Parcels
            .Where(p => p.Id == request.ParcelId &&
                        p.SenderId == senderContext.SenderId &&
                        p.State == ParcelState.Registered &&
                        !p.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException(nameof(Parcel), request.ParcelId);

        var receiverPoint = new Point((double)parcel.ReceiverLongitude, (double)parcel.ReceiverLatitude) { SRID = 4326 };

        var matchedZone = await context.CourierZones
            .Where(z =>
                z.CourierId == request.CourierId &&
                z.ZoneType == ZoneType.Distribution &&
                !z.IsDeleted &&
                z.Courier.IsActive && !z.Courier.IsDeleted &&
                (
                    (z.Boundary != null && z.Boundary.Contains(receiverPoint)) ||
                    (z.CenterPoint != null && z.CenterPoint.Distance(receiverPoint) <= (double)z.Radius * 1000)
                )
            )
            .Include(z => z.Courier)
                .ThenInclude(c => c.SlaConfigs)
            .OrderBy(z => z.Price ?? decimal.MaxValue)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new InvalidOperationException("Selected courier is no longer eligible for this parcel's destination.");

        var courier = matchedZone.Courier;
        var slaConfig = courier.SlaConfigs
            .Where(s => s.ServiceType == ServiceType.LastMile && !s.IsDeleted)
            .OrderBy(s => s.SlaHours)
            .FirstOrDefault()
            ?? throw new InvalidOperationException("Courier has no LastMile SLA configuration.");

        var now = dateTime.UtcNow;
        var userId = currentUser.UserId ?? 0;

        var distributionPrice = matchedZone.Price ?? courier.BasePrice;
        var extras = (parcel.HasCOD ? courier.CodPrice : 0)
                   + (parcel.HasFreightCollect ? courier.FreightCollectPrice : 0);

        var parcelCourier = new ParcelCourier
        {
            ParcelId = parcel.Id,
            CourierId = courier.Id,
            ServiceType = ServiceType.LastMile,
            LNumber = 0,
            Status = ParcelTrackingEnum.Processing,
            CollectFirstName = parcel.SenderFirstName,
            CollectLastName = parcel.SenderLastName,
            CollectPhoneNumber = parcel.SenderPhoneNumber,
            CollectLatitude = parcel.SenderLatitude,
            CollectLongitude = parcel.SenderLongitude,
            CollectAddress = parcel.SenderAddress,
            DistributeFirstName = parcel.ReceiverFirstName,
            DistributeLastName = parcel.ReceiverLastName,
            DistributePhoneNumber = parcel.ReceiverPhoneNumber,
            DistributeLatitude = parcel.ReceiverLatitude,
            DistributeLongitude = parcel.ReceiverLongitude,
            DistributeAddress = parcel.ReceiverAddress,
            AssignedAt = now,
            PromisedDeliveryAt = now.AddHours(slaConfig.SlaHours),
            CreatedByUserId = userId,
            ModifiedByUserId = userId,
            ModifiedDateTime = now,
            CreatedAt = now,
            IsActive = true
        };

        context.ParcelCouriers.Add(parcelCourier);
        await context.SaveChangesAsync(cancellationToken);

        context.ParcelCosts.Add(new ParcelCost
        {
            ParcelCourierId = parcelCourier.Id,
            PickupCost = 0,
            DistributionCost = distributionPrice + extras,
            CreatedAt = now,
            CreatedByUserId = userId
        });

        parcel.State = ParcelState.Confirmed;
        parcel.ModifiedByUserId = userId;
        parcel.ModifiedDateTime = now;

        await context.SaveChangesAsync(cancellationToken);
    }
}

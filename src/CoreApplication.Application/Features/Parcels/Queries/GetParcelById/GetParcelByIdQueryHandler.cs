using CoreApplication.Application.Common.Exceptions;
using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Application.Features.Parcels.Common;
using CoreApplication.Domain.Entities.Shipment;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.Parcels.Queries.GetParcelById;

public class GetParcelByIdQueryHandler(IApplicationDbContext context, ISenderContext senderContext)
    : IRequestHandler<GetParcelByIdQuery, ParcelDetailDto>
{
    public async Task<ParcelDetailDto> Handle(GetParcelByIdQuery request, CancellationToken cancellationToken)
    {
        var parcel = await context.Parcels
            .Include(p => p.ParcelCouriers
                .Where(pc => pc.IsActive && !pc.IsDeleted))
                .ThenInclude(pc => pc.Cost)
            .Include(p => p.ParcelCouriers)
                .ThenInclude(pc => pc.Courier)
            .Where(p => p.Id == request.Id &&
                        p.SenderId == senderContext.SenderId &&
                        !p.IsDeleted)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException(nameof(Parcel), request.Id);

        var activeParcelCourier = parcel.ParcelCouriers
            .OrderByDescending(pc => pc.AssignedAt)
            .FirstOrDefault();

        ParcelCourierInfoDto? courierInfo = null;
        if (activeParcelCourier is not null)
        {
            courierInfo = new ParcelCourierInfoDto(
                activeParcelCourier.CourierId,
                activeParcelCourier.Courier.Title,
                activeParcelCourier.ServiceType,
                activeParcelCourier.Status,
                activeParcelCourier.Cost?.PickupCost ?? 0,
                activeParcelCourier.Cost?.DistributionCost ?? 0,
                activeParcelCourier.Cost?.TotalCost ?? 0,
                activeParcelCourier.AssignedAt,
                activeParcelCourier.PromisedDeliveryAt);
        }

        return new ParcelDetailDto(
            parcel.Id,
            parcel.Barcode,
            parcel.State,
            parcel.ParcelType,
            parcel.Description,
            parcel.Length,
            parcel.Width,
            parcel.Height,
            parcel.Weight,
            parcel.Value,
            parcel.SenderFirstName,
            parcel.SenderLastName,
            parcel.SenderPhoneNumber,
            parcel.SenderLatitude,
            parcel.SenderLongitude,
            parcel.SenderAddress,
            parcel.ReceiverFirstName,
            parcel.ReceiverLastName,
            parcel.ReceiverPhoneNumber,
            parcel.ReceiverLatitude,
            parcel.ReceiverLongitude,
            parcel.ReceiverAddress,
            parcel.HasCOD,
            parcel.HasFMCG,
            parcel.HasFreightCollect,
            parcel.CreatedAt,
            courierInfo);
    }
}

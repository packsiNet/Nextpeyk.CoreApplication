using CoreApplication.Application.Common.Exceptions;
using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Application.Features.CourierPanel.Common;
using CoreApplication.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.CourierPanel.Queries.GetProcessingParcels;

public class GetProcessingParcelsQueryHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser)
    : IRequestHandler<GetProcessingParcelsQuery, List<ProcessingParcelDto>>
{
    public async Task<List<ProcessingParcelDto>> Handle(GetProcessingParcelsQuery request, CancellationToken cancellationToken)
    {
        var courierId = currentUser.CourierId
            ?? throw new ForbiddenAccessException();

        return await context.ParcelCouriers
            .Where(pc =>
                pc.CourierId == courierId &&
                pc.Status == ParcelTrackingEnum.Processing &&
                !pc.IsDeleted &&
                pc.Parcel.IsActive && !pc.Parcel.IsDeleted)
            .Select(pc => new ProcessingParcelDto(
                pc.Id,
                pc.ParcelId,
                pc.Parcel.Barcode,
                pc.LNumber,
                pc.ServiceType,
                pc.DistributeFirstName,
                pc.DistributeLastName,
                pc.DistributePhoneNumber,
                (double)pc.DistributeLatitude,
                (double)pc.DistributeLongitude,
                pc.DistributeAddress,
                pc.Parcel.HasCOD,
                pc.Parcel.HasFMCG,
                pc.Parcel.HasFreightCollect,
                pc.PromisedDeliveryAt))
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}

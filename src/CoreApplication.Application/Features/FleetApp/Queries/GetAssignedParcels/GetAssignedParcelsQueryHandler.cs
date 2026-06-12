using CoreApplication.Application.Common.Exceptions;
using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Application.Features.FleetApp.Common;
using CoreApplication.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.FleetApp.Queries.GetAssignedParcels;

public class GetAssignedParcelsQueryHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser)
    : IRequestHandler<GetAssignedParcelsQuery, List<AssignedParcelDto>>
{
    public async Task<List<AssignedParcelDto>> Handle(GetAssignedParcelsQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId
            ?? throw new ForbiddenAccessException();

        var fleet = await context.Fleets
            .FirstOrDefaultAsync(f => f.UserAccountId == userId && f.IsActive && !f.IsDeleted, cancellationToken)
            ?? throw new NotFoundException("Fleet", userId);

        return await context.ParcelCourierFleets
            .Where(pcf =>
                pcf.FleetId == fleet.Id &&
                !pcf.IsDeleted &&
                (pcf.Status == ParcelTrackingEnum.AssignToFleet ||
                 pcf.Status == ParcelTrackingEnum.ReceivedByFleet))
            .OrderBy(pcf => pcf.SortOrder)
            .Select(pcf => new AssignedParcelDto(
                pcf.Id,
                pcf.ParcelCourierId,
                pcf.ParcelCourier.Parcel.Barcode,
                pcf.ParcelCourier.LNumber,
                pcf.ParcelCourier.ServiceType,
                pcf.Status,
                pcf.SortOrder,
                pcf.ParcelCourier.DistributeFirstName,
                pcf.ParcelCourier.DistributeLastName,
                pcf.ParcelCourier.DistributePhoneNumber,
                (double)pcf.ParcelCourier.DistributeLatitude,
                (double)pcf.ParcelCourier.DistributeLongitude,
                pcf.ParcelCourier.DistributeAddress,
                pcf.ParcelCourier.Parcel.HasCOD,
                pcf.ParcelCourier.Parcel.HasFMCG,
                pcf.ParcelCourier.Parcel.HasFreightCollect,
                pcf.ParcelCourier.PromisedDeliveryAt))
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}

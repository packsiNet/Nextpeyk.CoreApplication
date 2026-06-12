using CoreApplication.Application.Common.Exceptions;
using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Domain.Entities.Shipment;
using CoreApplication.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.CourierPanel.Commands.ReassignParcel;

public class ReassignParcelCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser,
    IDateTimeService dateTime)
    : IRequestHandler<ReassignParcelCommand>
{
    public async Task Handle(ReassignParcelCommand request, CancellationToken cancellationToken)
    {
        var courierId = currentUser.CourierId
            ?? throw new ForbiddenAccessException();

        var now = dateTime.UtcNow;
        var userId = currentUser.UserId ?? 0;

        var pcf = await context.ParcelCourierFleets
            .Include(x => x.ParcelCourier)
            .FirstOrDefaultAsync(x =>
                x.Id == request.ParcelCourierFleetId &&
                x.ParcelCourier.CourierId == courierId &&
                x.Status == ParcelTrackingEnum.AssignToFleet &&
                !x.IsDeleted,
                cancellationToken)
            ?? throw new NotFoundException(nameof(ParcelCourierFleet), request.ParcelCourierFleetId);

        pcf.Status = ParcelTrackingEnum.AssignCancelled;
        pcf.ModifiedByUserId = userId;
        pcf.ModifiedDateTime = now;

        // Return parcel to Processing so it reappears on the map
        pcf.ParcelCourier.Status = ParcelTrackingEnum.Processing;
        pcf.ParcelCourier.ModifiedByUserId = userId;
        pcf.ParcelCourier.ModifiedDateTime = now;

        await context.SaveChangesAsync(cancellationToken);
    }
}

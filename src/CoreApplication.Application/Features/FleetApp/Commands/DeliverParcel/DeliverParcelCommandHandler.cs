using CoreApplication.Application.Common.Exceptions;
using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Domain.Entities.Shipment;
using CoreApplication.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.FleetApp.Commands.DeliverParcel;

public class DeliverParcelCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser,
    IDateTimeService dateTime)
    : IRequestHandler<DeliverParcelCommand>
{
    public async Task Handle(DeliverParcelCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId
            ?? throw new ForbiddenAccessException();

        var now = dateTime.UtcNow;

        var fleet = await context.Fleets
            .FirstOrDefaultAsync(f => f.UserAccountId == userId && f.IsActive && !f.IsDeleted, cancellationToken)
            ?? throw new NotFoundException("Fleet", userId);

        var pcf = await context.ParcelCourierFleets
            .Include(x => x.ParcelCourier)
            .FirstOrDefaultAsync(x =>
                x.Id == request.ParcelCourierFleetId &&
                x.FleetId == fleet.Id &&
                x.Status == ParcelTrackingEnum.ReceivedByFleet &&
                !x.IsDeleted,
                cancellationToken)
            ?? throw new NotFoundException(nameof(ParcelCourierFleet), request.ParcelCourierFleetId);

        pcf.Status = ParcelTrackingEnum.FinalizedDelivery;
        pcf.DeliveredAt = now;
        pcf.ReceiverName = request.ReceiverName;
        pcf.AuthType = request.AuthType;
        pcf.Signature = request.Signature;
        pcf.PODCode = request.PODCode;
        pcf.ModifiedByUserId = userId;
        pcf.ModifiedDateTime = now;

        pcf.ParcelCourier.Status = ParcelTrackingEnum.FinalizedDelivery;
        pcf.ParcelCourier.ModifiedByUserId = userId;
        pcf.ParcelCourier.ModifiedDateTime = now;

        await context.SaveChangesAsync(cancellationToken);
    }
}

using CoreApplication.Application.Common.Exceptions;
using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Domain.Entities.Shipment;
using CoreApplication.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.FleetApp.Commands.ReturnParcel;

public class ReturnParcelCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser,
    IDateTimeService dateTime)
    : IRequestHandler<ReturnParcelCommand>
{
    public async Task Handle(ReturnParcelCommand request, CancellationToken cancellationToken)
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

        pcf.Status = ParcelTrackingEnum.Returned;
        pcf.ReturnReason = request.ReturnReason;
        pcf.ReturnNote = request.ReturnNote;
        pcf.ModifiedByUserId = userId;
        pcf.ModifiedDateTime = now;

        pcf.ParcelCourier.Status = ParcelTrackingEnum.Returned;
        pcf.ParcelCourier.ModifiedByUserId = userId;
        pcf.ParcelCourier.ModifiedDateTime = now;

        foreach (var path in request.ImagePaths)
        {
            context.ParcelCourierFleetImages.Add(new ParcelCourierFleetImage
            {
                ParcelCourierFleetId = pcf.Id,
                ImagePath = path,
                Status = ParcelTrackingEnum.Returned,
                CreatedAt = now,
                CreatedByUserId = userId
            });
        }

        await context.SaveChangesAsync(cancellationToken);
    }
}

using CoreApplication.Application.Common.Exceptions;
using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.FleetApp.Commands.AcceptParcels;

public class AcceptParcelsCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser,
    IDateTimeService dateTime)
    : IRequestHandler<AcceptParcelsCommand>
{
    public async Task Handle(AcceptParcelsCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId
            ?? throw new ForbiddenAccessException();

        var now = dateTime.UtcNow;

        var fleet = await context.Fleets
            .FirstOrDefaultAsync(f => f.UserAccountId == userId && f.IsActive && !f.IsDeleted, cancellationToken)
            ?? throw new NotFoundException("Fleet", userId);

        var records = await context.ParcelCourierFleets
            .Include(pcf => pcf.ParcelCourier)
            .Where(pcf =>
                request.ParcelCourierFleetIds.Contains(pcf.Id) &&
                pcf.FleetId == fleet.Id &&
                pcf.Status == ParcelTrackingEnum.AssignToFleet &&
                !pcf.IsDeleted)
            .ToListAsync(cancellationToken);

        if (records.Count != request.ParcelCourierFleetIds.Count)
            throw new InvalidOperationException(
                "One or more parcels not found, already accepted, or do not belong to this fleet.");

        foreach (var pcf in records)
        {
            pcf.Status = ParcelTrackingEnum.ReceivedByFleet;
            pcf.ModifiedByUserId = userId;
            pcf.ModifiedDateTime = now;

            pcf.ParcelCourier.Status = ParcelTrackingEnum.ReceivedByFleet;
            pcf.ParcelCourier.ModifiedByUserId = userId;
            pcf.ParcelCourier.ModifiedDateTime = now;
        }

        await context.SaveChangesAsync(cancellationToken);
    }
}

using CoreApplication.Application.Common.Exceptions;
using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.CourierPanel.Commands.CancelFleetAssignments;

public class CancelFleetAssignmentsCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser,
    IDateTimeService dateTime)
    : IRequestHandler<CancelFleetAssignmentsCommand, int>
{
    public async Task<int> Handle(
        CancelFleetAssignmentsCommand request,
        CancellationToken cancellationToken)
    {
        var courierId = currentUser.CourierId ?? throw new ForbiddenAccessException();
        var userId = currentUser.UserId ?? 0;
        var now = dateTime.UtcNow;

        // Security: fleet must belong to this courier
        var fleetExists = await context.Fleets
            .AnyAsync(f => f.Id == request.FleetId && f.CourierId == courierId && !f.IsDeleted,
                cancellationToken);

        if (!fleetExists)
            throw new NotFoundException("Fleet", request.FleetId);

        var todayStart = now.Date;
        var todayEnd = todayStart.AddDays(1);

        // Only AssignToFleet (not yet accepted by driver) assigned today
        var records = await context.ParcelCourierFleets
            .Include(f => f.ParcelCourier)
            .Where(f =>
                f.FleetId == request.FleetId &&
                f.Status == ParcelTrackingEnum.AssignToFleet &&
                f.CreatedAt >= todayStart && f.CreatedAt < todayEnd &&
                !f.IsDeleted)
            .ToListAsync(cancellationToken);

        if (records.Count == 0) return 0;

        foreach (var pcf in records)
        {
            pcf.Status = ParcelTrackingEnum.AssignCancelled;
            pcf.ModifiedByUserId = userId;
            pcf.ModifiedDateTime = now;

            pcf.ParcelCourier.Status = ParcelTrackingEnum.Processing;
            pcf.ParcelCourier.ModifiedByUserId = userId;
            pcf.ParcelCourier.ModifiedDateTime = now;
        }

        await context.SaveChangesAsync(cancellationToken);
        return records.Count;
    }
}

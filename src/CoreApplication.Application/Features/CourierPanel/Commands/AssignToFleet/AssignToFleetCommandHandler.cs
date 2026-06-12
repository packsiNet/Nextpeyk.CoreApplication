using CoreApplication.Application.Common.Exceptions;
using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Domain.Entities.Shipment;
using CoreApplication.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.CourierPanel.Commands.AssignToFleet;

public class AssignToFleetCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser,
    IDateTimeService dateTime)
    : IRequestHandler<AssignToFleetCommand>
{
    public async Task Handle(AssignToFleetCommand request, CancellationToken cancellationToken)
    {
        var courierId = currentUser.CourierId
            ?? throw new ForbiddenAccessException();

        var now = dateTime.UtcNow;
        var userId = currentUser.UserId ?? 0;

        // Validate fleet belongs to this courier and is active
        var fleet = await context.Fleets
            .FirstOrDefaultAsync(f =>
                f.Id == request.FleetId &&
                f.CourierId == courierId &&
                f.IsActive && !f.IsDeleted,
                cancellationToken)
            ?? throw new NotFoundException("Fleet", request.FleetId);

        // Load all target ParcelCourier records — must belong to this courier and be in Processing
        var parcelCouriers = await context.ParcelCouriers
            .Where(pc =>
                request.ParcelCourierIds.Contains(pc.Id) &&
                pc.CourierId == courierId &&
                pc.Status == ParcelTrackingEnum.Processing &&
                !pc.IsDeleted)
            .ToListAsync(cancellationToken);

        if (parcelCouriers.Count != request.ParcelCourierIds.Count)
            throw new InvalidOperationException(
                "One or more parcels are not eligible for assignment (not found, wrong courier, or not in Processing status).");

        for (var i = 0; i < parcelCouriers.Count; i++)
        {
            var pc = parcelCouriers[i];

            context.ParcelCourierFleets.Add(new ParcelCourierFleet
            {
                ParcelCourierId = pc.Id,
                FleetId = fleet.Id,
                SortOrder = i,
                Status = ParcelTrackingEnum.AssignToFleet,
                IsDailySettlement = false,
                CreatedByUserId = userId,
                ModifiedByUserId = userId,
                ModifiedDateTime = now,
                CreatedAt = now,
                IsActive = true
            });

            pc.Status = ParcelTrackingEnum.AssignToFleet;
            pc.ModifiedByUserId = userId;
            pc.ModifiedDateTime = now;
        }

        await context.SaveChangesAsync(cancellationToken);
    }
}

using CoreApplication.Application.Common.Exceptions;
using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Domain.Entities.Fleet;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.Fleets.Commands.UpdateFleet;

public class UpdateFleetCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser,
    IDateTimeService dateTime)
    : IRequestHandler<UpdateFleetCommand>
{
    public async Task Handle(UpdateFleetCommand request, CancellationToken cancellationToken)
    {
        var courierId = currentUser.CourierId
            ?? throw new ForbiddenAccessException();

        var fleet = await context.Fleets
            .FirstOrDefaultAsync(f => f.Id == request.Id && f.CourierId == courierId && !f.IsDeleted, cancellationToken)
            ?? throw new NotFoundException(nameof(FleetEntity), request.Id);

        var fleetTypeExists = await context.FleetTypes
            .AnyAsync(ft => ft.Id == request.FleetTypeId && ft.IsActive && !ft.IsDeleted, cancellationToken);

        if (!fleetTypeExists)
            throw new NotFoundException("FleetType", request.FleetTypeId);

        var now = dateTime.UtcNow;
        var userId = currentUser.UserId ?? 0;

        fleet.FleetTypeId = request.FleetTypeId;
        fleet.Plaque = request.Plaque;
        fleet.DrivingLicense = request.DrivingLicense;
        fleet.StartDate = request.StartDate;
        fleet.EndDate = request.EndDate;
        fleet.Description = request.Description;
        fleet.InsuranceCode = request.InsuranceCode;
        fleet.IsActive = request.IsActive;
        fleet.ModifiedByUserId = userId;
        fleet.ModifiedDateTime = now;

        await context.SaveChangesAsync(cancellationToken);
    }
}

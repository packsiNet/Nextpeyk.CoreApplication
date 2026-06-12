using CoreApplication.Application.Common.Exceptions;
using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Domain.Entities.Fleet;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.Fleets.Commands.DeleteFleet;

public class DeleteFleetCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser,
    IDateTimeService dateTime)
    : IRequestHandler<DeleteFleetCommand>
{
    public async Task Handle(DeleteFleetCommand request, CancellationToken cancellationToken)
    {
        var courierId = currentUser.CourierId
            ?? throw new ForbiddenAccessException();

        var fleet = await context.Fleets
            .FirstOrDefaultAsync(f => f.Id == request.Id && f.CourierId == courierId && !f.IsDeleted, cancellationToken)
            ?? throw new NotFoundException(nameof(FleetEntity), request.Id);

        fleet.IsDeleted = true;
        fleet.IsActive = false;
        fleet.ModifiedByUserId = currentUser.UserId ?? 0;
        fleet.ModifiedDateTime = dateTime.UtcNow;

        await context.SaveChangesAsync(cancellationToken);
    }
}

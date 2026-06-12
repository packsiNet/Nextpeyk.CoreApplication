using CoreApplication.Application.Common.Exceptions;
using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Domain.Entities.Fleet;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.Fleets.Commands.DeleteFleetType;

public class DeleteFleetTypeCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser,
    IDateTimeService dateTime)
    : IRequestHandler<DeleteFleetTypeCommand>
{
    public async Task Handle(DeleteFleetTypeCommand request, CancellationToken cancellationToken)
    {
        var fleetType = await context.FleetTypes
            .FirstOrDefaultAsync(ft => ft.Id == request.Id && !ft.IsDeleted, cancellationToken)
            ?? throw new NotFoundException(nameof(FleetType), request.Id);

        fleetType.IsDeleted = true;
        fleetType.IsActive = false;
        fleetType.ModifiedByUserId = currentUser.UserId ?? 0;
        fleetType.ModifiedDateTime = dateTime.UtcNow;

        await context.SaveChangesAsync(cancellationToken);
    }
}

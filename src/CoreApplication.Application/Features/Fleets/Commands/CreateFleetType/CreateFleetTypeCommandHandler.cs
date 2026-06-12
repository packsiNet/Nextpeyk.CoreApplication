using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Domain.Entities.Fleet;
using MediatR;

namespace CoreApplication.Application.Features.Fleets.Commands.CreateFleetType;

public class CreateFleetTypeCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser,
    IDateTimeService dateTime)
    : IRequestHandler<CreateFleetTypeCommand, int>
{
    public async Task<int> Handle(CreateFleetTypeCommand request, CancellationToken cancellationToken)
    {
        var now = dateTime.UtcNow;
        var userId = currentUser.UserId ?? 0;

        var fleetType = new FleetType
        {
            Title = request.Title,
            CreatedByUserId = userId,
            ModifiedByUserId = userId,
            ModifiedDateTime = now,
            CreatedAt = now,
            IsActive = true
        };

        context.FleetTypes.Add(fleetType);
        await context.SaveChangesAsync(cancellationToken);

        return fleetType.Id;
    }
}

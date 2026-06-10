using CoreApplication.Application.Common.Exceptions;
using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Domain.Entities.Courier;
using MediatR;

namespace CoreApplication.Application.Features.Couriers.Commands.DeleteCourierZone;

public class DeleteCourierZoneCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser,
    IDateTimeService dateTime)
    : IRequestHandler<DeleteCourierZoneCommand>
{
    public async Task Handle(DeleteCourierZoneCommand request, CancellationToken cancellationToken)
    {
        var zone = await context.CourierZones.FindAsync([request.Id], cancellationToken)
            ?? throw new NotFoundException(nameof(CourierZone), request.Id);

        zone.IsDeleted = true;
        zone.IsActive = false;
        zone.ModifiedByUserId = currentUser.UserId ?? 0;
        zone.ModifiedDateTime = dateTime.UtcNow;

        await context.SaveChangesAsync(cancellationToken);
    }
}

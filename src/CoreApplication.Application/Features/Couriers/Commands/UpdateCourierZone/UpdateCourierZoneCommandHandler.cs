using CoreApplication.Application.Common.Exceptions;
using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Domain.Entities.Courier;
using MediatR;

namespace CoreApplication.Application.Features.Couriers.Commands.UpdateCourierZone;

public class UpdateCourierZoneCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser,
    IDateTimeService dateTime)
    : IRequestHandler<UpdateCourierZoneCommand>
{
    public async Task Handle(UpdateCourierZoneCommand request, CancellationToken cancellationToken)
    {
        var zone = await context.CourierZones.FindAsync([request.Id], cancellationToken)
            ?? throw new NotFoundException(nameof(CourierZone), request.Id);

        if (zone.IsDeleted)
            throw new NotFoundException(nameof(CourierZone), request.Id);

        zone.ZoneType = request.ZoneType;
        zone.Name = request.Name;
        zone.CityId = request.CityId;
        zone.Boundary = request.Boundary;
        zone.CenterPoint = request.CenterPoint;
        zone.Radius = request.Radius;
        zone.Price = request.Price;
        zone.ModifiedByUserId = currentUser.UserId ?? 0;
        zone.ModifiedDateTime = dateTime.UtcNow;

        await context.SaveChangesAsync(cancellationToken);
    }
}

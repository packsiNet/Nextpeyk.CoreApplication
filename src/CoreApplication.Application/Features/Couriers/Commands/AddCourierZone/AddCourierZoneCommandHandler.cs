using CoreApplication.Application.Common.Exceptions;
using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Domain.Entities.Courier;
using MediatR;

namespace CoreApplication.Application.Features.Couriers.Commands.AddCourierZone;

public class AddCourierZoneCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser,
    IDateTimeService dateTime)
    : IRequestHandler<AddCourierZoneCommand, int>
{
    public async Task<int> Handle(AddCourierZoneCommand request, CancellationToken cancellationToken)
    {
        var courierExists = await context.Couriers.FindAsync([request.CourierId], cancellationToken);
        if (courierExists is null || courierExists.IsDeleted)
            throw new NotFoundException(nameof(Domain.Entities.Courier.Courier), request.CourierId);

        var now = dateTime.UtcNow;
        var userId = currentUser.UserId ?? 0;

        var zone = new CourierZone
        {
            CourierId = request.CourierId,
            ZoneType = request.ZoneType,
            Name = request.Name,
            CityId = request.CityId,
            Boundary = request.Boundary,
            CenterPoint = request.CenterPoint,
            Radius = request.Radius,
            Price = request.Price,
            CreatedByUserId = userId,
            ModifiedByUserId = userId,
            ModifiedDateTime = now,
            CreatedAt = now,
            IsActive = true
        };

        context.CourierZones.Add(zone);
        await context.SaveChangesAsync(cancellationToken);

        return zone.Id;
    }
}

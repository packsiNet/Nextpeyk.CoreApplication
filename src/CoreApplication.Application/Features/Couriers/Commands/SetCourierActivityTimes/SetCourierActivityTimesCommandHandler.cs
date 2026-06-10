using CoreApplication.Application.Common.Exceptions;
using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Domain.Entities.Courier;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.Couriers.Commands.SetCourierActivityTimes;

public class SetCourierActivityTimesCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser,
    IDateTimeService dateTime)
    : IRequestHandler<SetCourierActivityTimesCommand>
{
    public async Task Handle(SetCourierActivityTimesCommand request, CancellationToken cancellationToken)
    {
        var courierExists = await context.Couriers.FindAsync([request.CourierId], cancellationToken);
        if (courierExists is null || courierExists.IsDeleted)
            throw new NotFoundException(nameof(Domain.Entities.Courier.Courier), request.CourierId);

        var now = dateTime.UtcNow;
        var userId = currentUser.UserId ?? 0;

        var existing = await context.CourierActivityTimes
            .Where(a => a.CourierId == request.CourierId && !a.IsDeleted)
            .ToListAsync(cancellationToken);

        foreach (var item in existing)
        {
            item.IsDeleted = true;
            item.IsActive = false;
            item.ModifiedByUserId = userId;
            item.ModifiedDateTime = now;
        }

        foreach (var item in request.ActivityTimes)
        {
            context.CourierActivityTimes.Add(new CourierActivityTime
            {
                CourierId = request.CourierId,
                DayOfWeek = item.DayOfWeek,
                StartTime = item.StartTime,
                EndTime = item.EndTime,
                CreatedByUserId = userId,
                ModifiedByUserId = userId,
                ModifiedDateTime = now,
                CreatedAt = now,
                IsActive = true
            });
        }

        await context.SaveChangesAsync(cancellationToken);
    }
}

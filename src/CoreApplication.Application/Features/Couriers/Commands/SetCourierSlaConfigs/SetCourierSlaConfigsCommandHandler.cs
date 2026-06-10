using CoreApplication.Application.Common.Exceptions;
using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Domain.Entities.Courier;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.Couriers.Commands.SetCourierSlaConfigs;

public class SetCourierSlaConfigsCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser,
    IDateTimeService dateTime)
    : IRequestHandler<SetCourierSlaConfigsCommand>
{
    public async Task Handle(SetCourierSlaConfigsCommand request, CancellationToken cancellationToken)
    {
        var courierExists = await context.Couriers.FindAsync([request.CourierId], cancellationToken);
        if (courierExists is null || courierExists.IsDeleted)
            throw new NotFoundException(nameof(Domain.Entities.Courier.Courier), request.CourierId);

        var now = dateTime.UtcNow;
        var userId = currentUser.UserId ?? 0;

        var existing = await context.CourierSlaConfigs
            .Where(s => s.CourierId == request.CourierId && !s.IsDeleted)
            .ToListAsync(cancellationToken);

        foreach (var item in existing)
        {
            item.IsDeleted = true;
            item.IsActive = false;
            item.ModifiedByUserId = userId;
            item.ModifiedDateTime = now;
        }

        foreach (var item in request.SlaConfigs)
        {
            context.CourierSlaConfigs.Add(new CourierSlaConfig
            {
                CourierId = request.CourierId,
                ServiceType = item.ServiceType,
                SlaHours = item.SlaHours,
                SuccessRateMin = item.SuccessRateMin,
                OnTimeMin = item.OnTimeMin,
                ReturnRateMax = item.ReturnRateMax,
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

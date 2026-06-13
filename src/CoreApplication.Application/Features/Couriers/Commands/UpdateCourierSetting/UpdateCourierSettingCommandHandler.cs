using CoreApplication.Application.Common.Exceptions;
using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Domain.Entities.Courier;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.Couriers.Commands.UpdateCourierSetting;

public class UpdateCourierSettingCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser,
    IDateTimeService dateTime)
    : IRequestHandler<UpdateCourierSettingCommand>
{
    public async Task Handle(UpdateCourierSettingCommand request, CancellationToken cancellationToken)
    {
        var courierExists = await context.Couriers
            .AnyAsync(c => c.Id == request.CourierId && !c.IsDeleted, cancellationToken);

        if (!courierExists)
            throw new NotFoundException(nameof(Domain.Entities.Courier.Courier), request.CourierId);

        var now = dateTime.UtcNow;
        var userId = currentUser.UserId ?? 0;

        var setting = await context.CourierSettings
            .Where(s => s.CourierId == request.CourierId && !s.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);

        if (setting is not null)
        {
            setting.IsGroupAcceptance = request.IsGroupAcceptance;
            setting.ModifiedByUserId = userId;
            setting.ModifiedDateTime = now;
        }
        else
        {
            context.CourierSettings.Add(new CourierSetting
            {
                CourierId = request.CourierId,
                IsGroupAcceptance = request.IsGroupAcceptance,
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

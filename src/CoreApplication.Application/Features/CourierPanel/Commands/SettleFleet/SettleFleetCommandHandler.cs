using CoreApplication.Application.Common.Exceptions;
using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.CourierPanel.Commands.SettleFleet;

public class SettleFleetCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser,
    IDateTimeService dateTime)
    : IRequestHandler<SettleFleetCommand, int>
{
    public async Task<int> Handle(SettleFleetCommand request, CancellationToken cancellationToken)
    {
        var courierId = currentUser.CourierId
            ?? throw new ForbiddenAccessException();

        var now = dateTime.UtcNow;
        var userId = currentUser.UserId ?? 0;
        var settleDate = request.Date ?? DateOnly.FromDateTime(now);

        var fleet = await context.Fleets
            .FirstOrDefaultAsync(f =>
                f.Id == request.FleetId &&
                f.CourierId == courierId &&
                f.IsActive && !f.IsDeleted,
                cancellationToken)
            ?? throw new NotFoundException("Fleet", request.FleetId);

        var periodStart = settleDate.ToDateTime(TimeOnly.MinValue);
        var periodEnd = settleDate.ToDateTime(TimeOnly.MaxValue);

        var pending = await context.ParcelCourierFleets
            .Where(pcf =>
                pcf.FleetId == fleet.Id &&
                !pcf.IsDailySettlement &&
                !pcf.IsDeleted &&
                (pcf.Status == ParcelTrackingEnum.FinalizedDelivery ||
                 pcf.Status == ParcelTrackingEnum.Returned) &&
                pcf.ModifiedDateTime >= periodStart &&
                pcf.ModifiedDateTime <= periodEnd)
            .ToListAsync(cancellationToken);

        foreach (var pcf in pending)
        {
            pcf.IsDailySettlement = true;
            pcf.ModifiedByUserId = userId;
            pcf.ModifiedDateTime = now;
        }

        await context.SaveChangesAsync(cancellationToken);

        return pending.Count;
    }
}

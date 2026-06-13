using CoreApplication.Application.Common.Exceptions;
using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.CourierPanel.Commands.ConfirmAbsentParcels;

public class ConfirmAbsentParcelsCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser,
    IDateTimeService dateTime)
    : IRequestHandler<ConfirmAbsentParcelsCommand, int>
{
    public async Task<int> Handle(
        ConfirmAbsentParcelsCommand request,
        CancellationToken cancellationToken)
    {
        var courierId = currentUser.CourierId ?? throw new ForbiddenAccessException();
        var userId = currentUser.UserId ?? 0;
        var now = dateTime.UtcNow;

        // Only Processing parcels belonging to this courier — security + state guard
        var parcels = await context.ParcelCouriers
            .Where(pc =>
                request.ParcelCourierIds.Contains(pc.Id) &&
                pc.CourierId == courierId &&
                pc.Status == ParcelTrackingEnum.Processing &&
                !pc.IsDeleted)
            .ToListAsync(cancellationToken);

        if (parcels.Count == 0) return 0;

        foreach (var pc in parcels)
        {
            pc.Status = ParcelTrackingEnum.PhysicallyAbsent;
            pc.ModifiedByUserId = userId;
            pc.ModifiedDateTime = now;
        }

        await context.SaveChangesAsync(cancellationToken);
        return parcels.Count;
    }
}

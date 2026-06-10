using CoreApplication.Application.Common.Exceptions;
using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Domain.Entities.Courier;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.Couriers.Commands.AssignBoxSizeToCourier;

public class AssignBoxSizeToCourierCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser,
    IDateTimeService dateTime)
    : IRequestHandler<AssignBoxSizeToCourierCommand, int>
{
    public async Task<int> Handle(AssignBoxSizeToCourierCommand request, CancellationToken cancellationToken)
    {
        var courierExists = await context.Couriers.FindAsync([request.CourierId], cancellationToken);
        if (courierExists is null || courierExists.IsDeleted)
            throw new NotFoundException(nameof(Domain.Entities.Courier.Courier), request.CourierId);

        var boxSizeExists = await context.BoxSizes.FindAsync([request.BoxSizeId], cancellationToken);
        if (boxSizeExists is null || boxSizeExists.IsDeleted)
            throw new NotFoundException(nameof(BoxSize), request.BoxSizeId);

        var alreadyAssigned = await context.CourierBoxSizes
            .AnyAsync(b => b.CourierId == request.CourierId && b.BoxSizeId == request.BoxSizeId && !b.IsDeleted, cancellationToken);

        if (alreadyAssigned)
            throw new InvalidOperationException("Box size already assigned to this courier.");

        var now = dateTime.UtcNow;
        var userId = currentUser.UserId ?? 0;

        var assignment = new CourierBoxSize
        {
            CourierId = request.CourierId,
            BoxSizeId = request.BoxSizeId,
            PackagingPrice = request.PackagingPrice,
            CreatedByUserId = userId,
            ModifiedByUserId = userId,
            ModifiedDateTime = now,
            CreatedAt = now,
            IsActive = true
        };

        context.CourierBoxSizes.Add(assignment);
        await context.SaveChangesAsync(cancellationToken);

        return assignment.Id;
    }
}

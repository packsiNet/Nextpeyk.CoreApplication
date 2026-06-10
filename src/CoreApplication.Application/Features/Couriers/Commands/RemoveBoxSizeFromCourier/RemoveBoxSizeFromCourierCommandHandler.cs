using CoreApplication.Application.Common.Exceptions;
using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Domain.Entities.Courier;
using MediatR;

namespace CoreApplication.Application.Features.Couriers.Commands.RemoveBoxSizeFromCourier;

public class RemoveBoxSizeFromCourierCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser,
    IDateTimeService dateTime)
    : IRequestHandler<RemoveBoxSizeFromCourierCommand>
{
    public async Task Handle(RemoveBoxSizeFromCourierCommand request, CancellationToken cancellationToken)
    {
        var assignment = await context.CourierBoxSizes.FindAsync([request.CourierBoxSizeId], cancellationToken)
            ?? throw new NotFoundException(nameof(CourierBoxSize), request.CourierBoxSizeId);

        assignment.IsDeleted = true;
        assignment.IsActive = false;
        assignment.ModifiedByUserId = currentUser.UserId ?? 0;
        assignment.ModifiedDateTime = dateTime.UtcNow;

        await context.SaveChangesAsync(cancellationToken);
    }
}

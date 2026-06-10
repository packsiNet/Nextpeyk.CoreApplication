using CoreApplication.Application.Common.Exceptions;
using CoreApplication.Application.Common.Interfaces;
using MediatR;

namespace CoreApplication.Application.Features.Couriers.Commands.DeleteCourier;

public class DeleteCourierCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser,
    IDateTimeService dateTime)
    : IRequestHandler<DeleteCourierCommand>
{
    public async Task Handle(DeleteCourierCommand request, CancellationToken cancellationToken)
    {
        var courier = await context.Couriers.FindAsync([request.Id], cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Courier.Courier), request.Id);

        courier.IsDeleted = true;
        courier.IsActive = false;
        courier.ModifiedByUserId = currentUser.UserId ?? 0;
        courier.ModifiedDateTime = dateTime.UtcNow;

        await context.SaveChangesAsync(cancellationToken);
    }
}

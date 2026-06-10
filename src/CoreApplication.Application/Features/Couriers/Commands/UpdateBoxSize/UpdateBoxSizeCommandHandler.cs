using CoreApplication.Application.Common.Exceptions;
using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Domain.Entities.Courier;
using MediatR;

namespace CoreApplication.Application.Features.Couriers.Commands.UpdateBoxSize;

public class UpdateBoxSizeCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser,
    IDateTimeService dateTime)
    : IRequestHandler<UpdateBoxSizeCommand>
{
    public async Task Handle(UpdateBoxSizeCommand request, CancellationToken cancellationToken)
    {
        var boxSize = await context.BoxSizes.FindAsync([request.Id], cancellationToken)
            ?? throw new NotFoundException(nameof(BoxSize), request.Id);

        if (boxSize.IsDeleted)
            throw new NotFoundException(nameof(BoxSize), request.Id);

        boxSize.Order = request.Order;
        boxSize.Title = request.Title;
        boxSize.Code = request.Code;
        boxSize.BoxLength = request.BoxLength;
        boxSize.BoxWidth = request.BoxWidth;
        boxSize.BoxHeight = request.BoxHeight;
        boxSize.ModifiedByUserId = currentUser.UserId ?? 0;
        boxSize.ModifiedDateTime = dateTime.UtcNow;

        await context.SaveChangesAsync(cancellationToken);
    }
}

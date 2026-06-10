using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Domain.Entities.Courier;
using MediatR;

namespace CoreApplication.Application.Features.Couriers.Commands.CreateBoxSize;

public class CreateBoxSizeCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser,
    IDateTimeService dateTime)
    : IRequestHandler<CreateBoxSizeCommand, int>
{
    public async Task<int> Handle(CreateBoxSizeCommand request, CancellationToken cancellationToken)
    {
        var boxSize = new BoxSize
        {
            Order = request.Order,
            Title = request.Title,
            Code = request.Code,
            BoxLength = request.BoxLength,
            BoxWidth = request.BoxWidth,
            BoxHeight = request.BoxHeight,
            CreatedByUserId = currentUser.UserId ?? 0,
            ModifiedByUserId = currentUser.UserId ?? 0,
            ModifiedDateTime = dateTime.UtcNow,
            CreatedAt = dateTime.UtcNow,
            IsActive = true
        };

        context.BoxSizes.Add(boxSize);
        await context.SaveChangesAsync(cancellationToken);

        return boxSize.Id;
    }
}

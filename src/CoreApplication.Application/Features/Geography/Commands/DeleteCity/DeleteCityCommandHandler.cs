using CoreApplication.Application.Common.Exceptions;
using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Domain.Entities.Geography;
using MediatR;

namespace CoreApplication.Application.Features.Geography.Commands.DeleteCity;

public class DeleteCityCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser,
    IDateTimeService dateTime)
    : IRequestHandler<DeleteCityCommand>
{
    public async Task Handle(DeleteCityCommand request, CancellationToken cancellationToken)
    {
        var city = await context.Cities.FindAsync([request.Id], cancellationToken)
            ?? throw new NotFoundException(nameof(City), request.Id);

        city.IsDeleted = true;
        city.IsActive = false;
        city.ModifiedByUserId = currentUser.UserId ?? 0;
        city.ModifiedDateTime = dateTime.UtcNow;

        await context.SaveChangesAsync(cancellationToken);
    }
}

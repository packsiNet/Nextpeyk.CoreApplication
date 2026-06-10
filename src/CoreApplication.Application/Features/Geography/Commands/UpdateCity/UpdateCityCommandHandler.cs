using CoreApplication.Application.Common.Exceptions;
using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Domain.Entities.Geography;
using MediatR;

namespace CoreApplication.Application.Features.Geography.Commands.UpdateCity;

public class UpdateCityCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser,
    IDateTimeService dateTime)
    : IRequestHandler<UpdateCityCommand>
{
    public async Task Handle(UpdateCityCommand request, CancellationToken cancellationToken)
    {
        var city = await context.Cities.FindAsync([request.Id], cancellationToken)
            ?? throw new NotFoundException(nameof(City), request.Id);

        if (city.IsDeleted)
            throw new NotFoundException(nameof(City), request.Id);

        city.Name = request.Name;
        city.Boundary = request.Boundary;
        city.ModifiedByUserId = currentUser.UserId ?? 0;
        city.ModifiedDateTime = dateTime.UtcNow;

        await context.SaveChangesAsync(cancellationToken);
    }
}

using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Domain.Entities.Geography;
using MediatR;

namespace CoreApplication.Application.Features.Geography.Commands.CreateCity;

public class CreateCityCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser,
    IDateTimeService dateTime)
    : IRequestHandler<CreateCityCommand, int>
{
    public async Task<int> Handle(CreateCityCommand request, CancellationToken cancellationToken)
    {
        var city = new City
        {
            Name = request.Name,
            Boundary = request.Boundary,
            CreatedByUserId = currentUser.UserId ?? 0,
            ModifiedByUserId = currentUser.UserId ?? 0,
            ModifiedDateTime = dateTime.UtcNow,
            CreatedAt = dateTime.UtcNow,
            IsActive = true
        };

        context.Cities.Add(city);
        await context.SaveChangesAsync(cancellationToken);

        return city.Id;
    }
}

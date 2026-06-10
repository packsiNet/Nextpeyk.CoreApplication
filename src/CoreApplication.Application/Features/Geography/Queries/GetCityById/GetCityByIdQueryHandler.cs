using CoreApplication.Application.Common.Exceptions;
using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Application.Features.Geography.Common;
using CoreApplication.Domain.Entities.Geography;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.Geography.Queries.GetCityById;

public class GetCityByIdQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetCityByIdQuery, CityDto>
{
    public async Task<CityDto> Handle(GetCityByIdQuery request, CancellationToken cancellationToken)
    {
        var city = await context.Cities
            .Where(c => c.Id == request.Id && !c.IsDeleted)
            .Select(c => new CityDto(c.Id, c.Name, c.Boundary))
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException(nameof(City), request.Id);

        return city;
    }
}

using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Application.Common.Models;
using CoreApplication.Application.Features.Geography.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.Geography.Queries.GetCities;

public class GetCitiesQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetCitiesQuery, PaginatedList<CityDto>>
{
    public async Task<PaginatedList<CityDto>> Handle(GetCitiesQuery request, CancellationToken cancellationToken)
    {
        var query = context.Cities
            .Where(c => !c.IsDeleted)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            query = query.Where(c => c.Name.Contains(request.SearchTerm));

        var projected = query
            .OrderBy(c => c.Name)
            .Select(c => new CityDto(c.Id, c.Name, c.Boundary));

        return await PaginatedList<CityDto>.CreateAsync(projected, request.PageNumber, request.PageSize, cancellationToken);
    }
}

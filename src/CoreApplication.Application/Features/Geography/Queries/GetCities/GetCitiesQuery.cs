using CoreApplication.Application.Common.Models;
using CoreApplication.Application.Features.Geography.Common;
using MediatR;

namespace CoreApplication.Application.Features.Geography.Queries.GetCities;

public record GetCitiesQuery(
    int PageNumber = 1,
    int PageSize = 20,
    string? SearchTerm = null) : IRequest<PaginatedList<CityDto>>;

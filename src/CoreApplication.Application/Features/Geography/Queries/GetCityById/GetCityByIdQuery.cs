using CoreApplication.Application.Features.Geography.Common;
using MediatR;

namespace CoreApplication.Application.Features.Geography.Queries.GetCityById;

public record GetCityByIdQuery(int Id) : IRequest<CityDto>;

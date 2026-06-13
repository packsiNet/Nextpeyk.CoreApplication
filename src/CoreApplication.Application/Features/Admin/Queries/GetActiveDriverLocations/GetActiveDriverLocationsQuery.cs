using MediatR;

namespace CoreApplication.Application.Features.Admin.Queries.GetActiveDriverLocations;

public record GetActiveDriverLocationsQuery(int? CourierId = null) : IRequest<List<ActiveDriverLocationDto>>;

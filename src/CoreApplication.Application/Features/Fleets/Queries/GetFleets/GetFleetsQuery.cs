using CoreApplication.Application.Features.Fleets.Common;
using MediatR;

namespace CoreApplication.Application.Features.Fleets.Queries.GetFleets;

public record GetFleetsQuery(bool? ActiveOnly = true) : IRequest<List<FleetListDto>>;

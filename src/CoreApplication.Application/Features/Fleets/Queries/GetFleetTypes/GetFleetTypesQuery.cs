using CoreApplication.Application.Features.Fleets.Common;
using MediatR;

namespace CoreApplication.Application.Features.Fleets.Queries.GetFleetTypes;

public record GetFleetTypesQuery : IRequest<List<FleetTypeDto>>;

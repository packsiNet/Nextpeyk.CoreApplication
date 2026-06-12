using CoreApplication.Application.Features.Fleets.Common;
using MediatR;

namespace CoreApplication.Application.Features.Fleets.Queries.GetFleetById;

public record GetFleetByIdQuery(int Id) : IRequest<FleetDetailDto>;

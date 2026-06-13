using CoreApplication.Application.Features.CourierPanel.Common;
using MediatR;

namespace CoreApplication.Application.Features.CourierPanel.Queries.GetFleetUnsettled;

public record GetFleetUnsettledQuery(
    int FleetId,
    DateOnly? Date = null) : IRequest<List<UnsettledParcelDto>>;

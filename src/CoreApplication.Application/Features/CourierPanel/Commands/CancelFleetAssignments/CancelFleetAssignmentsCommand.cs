using MediatR;

namespace CoreApplication.Application.Features.CourierPanel.Commands.CancelFleetAssignments;

public record CancelFleetAssignmentsCommand(int FleetId) : IRequest<int>;

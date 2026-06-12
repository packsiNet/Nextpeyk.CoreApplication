using MediatR;

namespace CoreApplication.Application.Features.CourierPanel.Commands.AssignToFleet;

public record AssignToFleetCommand(int FleetId, List<int> ParcelCourierIds) : IRequest;

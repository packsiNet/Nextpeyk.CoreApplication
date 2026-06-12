using MediatR;

namespace CoreApplication.Application.Features.Fleets.Commands.DeleteFleet;

public record DeleteFleetCommand(int Id) : IRequest;

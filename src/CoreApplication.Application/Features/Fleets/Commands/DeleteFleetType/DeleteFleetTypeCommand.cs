using MediatR;

namespace CoreApplication.Application.Features.Fleets.Commands.DeleteFleetType;

public record DeleteFleetTypeCommand(int Id) : IRequest;

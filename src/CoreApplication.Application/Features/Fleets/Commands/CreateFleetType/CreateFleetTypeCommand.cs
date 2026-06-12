using MediatR;

namespace CoreApplication.Application.Features.Fleets.Commands.CreateFleetType;

public record CreateFleetTypeCommand(string Title) : IRequest<int>;

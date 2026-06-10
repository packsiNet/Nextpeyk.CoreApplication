using MediatR;

namespace CoreApplication.Application.Features.Geography.Commands.UpdateCity;

public record UpdateCityCommand(int Id, string Name, string? Boundary) : IRequest;

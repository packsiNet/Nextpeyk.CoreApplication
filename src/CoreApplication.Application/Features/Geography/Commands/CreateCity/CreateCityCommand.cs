using MediatR;

namespace CoreApplication.Application.Features.Geography.Commands.CreateCity;

public record CreateCityCommand(string Name, string? Boundary) : IRequest<int>;

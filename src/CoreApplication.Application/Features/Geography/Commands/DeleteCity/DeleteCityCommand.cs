using MediatR;

namespace CoreApplication.Application.Features.Geography.Commands.DeleteCity;

public record DeleteCityCommand(int Id) : IRequest;

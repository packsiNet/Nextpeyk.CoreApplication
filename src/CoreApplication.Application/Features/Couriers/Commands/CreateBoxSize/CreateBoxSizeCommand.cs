using MediatR;

namespace CoreApplication.Application.Features.Couriers.Commands.CreateBoxSize;

public record CreateBoxSizeCommand(
    int Order,
    string Title,
    string Code,
    decimal BoxLength,
    decimal BoxWidth,
    decimal BoxHeight) : IRequest<int>;

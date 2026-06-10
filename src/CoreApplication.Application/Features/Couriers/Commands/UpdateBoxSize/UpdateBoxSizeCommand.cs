using MediatR;

namespace CoreApplication.Application.Features.Couriers.Commands.UpdateBoxSize;

public record UpdateBoxSizeCommand(
    int Id,
    int Order,
    string Title,
    string Code,
    decimal BoxLength,
    decimal BoxWidth,
    decimal BoxHeight) : IRequest;

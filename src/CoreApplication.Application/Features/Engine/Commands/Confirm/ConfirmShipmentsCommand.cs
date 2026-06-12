using CoreApplication.Application.Features.Engine.Common;
using MediatR;

namespace CoreApplication.Application.Features.Engine.Commands.Confirm;

public record ConfirmShipmentsCommand : IRequest<ConfirmResponseDto>
{
    public decimal Length { get; init; }
    public decimal Width { get; init; }
    public decimal Height { get; init; }
    public decimal Weight { get; init; }
    public List<ConfirmShipmentItemDto> Shipments { get; init; } = [];
}

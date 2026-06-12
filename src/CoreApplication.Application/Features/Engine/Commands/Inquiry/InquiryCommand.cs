using CoreApplication.Application.Features.Engine.Common;
using MediatR;

namespace CoreApplication.Application.Features.Engine.Commands.Inquiry;

public record InquiryCommand : IRequest<InquiryResponseDto>
{
    public decimal Length { get; init; }
    public decimal Width { get; init; }
    public decimal Height { get; init; }
    public decimal Weight { get; init; }
    public List<ShipmentInputDto> Shipments { get; init; } = [];
}

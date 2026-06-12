using CoreApplication.Domain.Enums;

namespace CoreApplication.Application.Features.Engine.Common;

public record ShipmentInputDto
{
    public int LNumber { get; init; }
    public ServiceType ServiceType { get; init; }
    public bool HasCOD { get; init; }
    public bool HasFMCG { get; init; }
    public bool HasFreightCollect { get; init; }
    public CoordinateDto Collection { get; init; } = default!;
    public CoordinateDto Distribution { get; init; } = default!;
}

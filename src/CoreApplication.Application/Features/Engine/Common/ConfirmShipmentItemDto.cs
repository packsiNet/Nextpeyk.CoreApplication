using CoreApplication.Domain.Enums;

namespace CoreApplication.Application.Features.Engine.Common;

public record ConfirmShipmentItemDto
{
    public int LNumber { get; init; }
    public ServiceType ServiceType { get; init; }
    public bool HasCOD { get; init; }
    public bool HasFMCG { get; init; }
    public bool HasFreightCollect { get; init; }
    public CoordinateDto Collection { get; init; } = default!;
    public CoordinateDto Distribution { get; init; } = default!;

    public ParcelType ParcelType { get; init; }
    public string? Description { get; init; }
    public decimal Value { get; init; }

    public string CollectFirstName { get; init; } = default!;
    public string CollectLastName { get; init; } = default!;
    public string CollectPhoneNumber { get; init; } = default!;
    public string CollectAddress { get; init; } = default!;

    public string DistributeFirstName { get; init; } = default!;
    public string DistributeLastName { get; init; } = default!;
    public string DistributePhoneNumber { get; init; } = default!;
    public string DistributeAddress { get; init; } = default!;
}

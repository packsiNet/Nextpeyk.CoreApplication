namespace CoreApplication.Application.Features.Couriers.Common;

public record CourierListDto(
    int Id,
    string Title,
    string? Description,
    string? Logo,
    string? SupportPhoneNumber,
    bool HasCOD,
    bool HasFMCG,
    bool HasFreightCollect,
    bool HasPackaging,
    bool IsActive);

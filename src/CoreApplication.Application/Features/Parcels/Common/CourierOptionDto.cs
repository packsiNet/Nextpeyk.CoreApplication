namespace CoreApplication.Application.Features.Parcels.Common;

public record CourierOptionDto(
    int CourierId,
    string Title,
    string? Logo,
    string? SupportPhoneNumber,
    int SlaHours,
    decimal BasePrice,
    decimal? ZonePrice,
    decimal EstimatedTotal);

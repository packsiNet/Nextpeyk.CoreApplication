namespace CoreApplication.Application.Features.Engine.Common;

public record ConfirmResponseDto(
    List<ConfirmedCourierDto> Accepted,
    RejectedItemsDto Rejected);

public record ConfirmedCourierDto(
    int CourierId,
    string CourierName,
    int Count,
    List<ConfirmedShipmentDetailDto> Details);

public record ConfirmedShipmentDetailDto(
    int LNumber,
    int ParcelId,
    string Barcode,
    int SlaHours,
    decimal EstimatedPrice);

namespace CoreApplication.Application.Features.Engine.Common;

public record InquiryResponseDto(
    List<AcceptedCourierDto> Accepted,
    RejectedItemsDto Rejected);

public record AcceptedCourierDto(
    int CourierId,
    string CourierName,
    int Count,
    List<AcceptedShipmentDetailDto> Details);

public record AcceptedShipmentDetailDto(
    int LNumber,
    int SlaHours,
    decimal EstimatedPrice);

public record RejectedItemsDto(
    int Count,
    List<RejectedShipmentDetailDto> Details);

public record RejectedShipmentDetailDto(
    int LNumber,
    string Reason);

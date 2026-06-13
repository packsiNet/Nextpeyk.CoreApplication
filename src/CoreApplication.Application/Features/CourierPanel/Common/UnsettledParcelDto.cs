using CoreApplication.Domain.Enums;

namespace CoreApplication.Application.Features.CourierPanel.Common;

public record UnsettledParcelDto(
    int ParcelCourierFleetId,
    int ParcelCourierId,
    string Barcode,
    ParcelTrackingEnum Status,
    string ReceiverFirstName,
    string ReceiverLastName,
    string DistributeAddress,
    bool HasCOD,
    decimal ParcelValue,
    DateTime? DeliveredAt,
    ReturnParcelReason? ReturnReason,
    string? ReturnNote);

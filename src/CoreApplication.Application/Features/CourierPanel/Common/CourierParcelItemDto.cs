using CoreApplication.Domain.Enums;

namespace CoreApplication.Application.Features.CourierPanel.Common;

public record CourierParcelItemDto(
    int ParcelCourierId,
    int ParcelId,
    string Barcode,
    ParcelTrackingEnum Status,
    string ReceiverFirstName,
    string ReceiverLastName,
    string ReceiverPhoneNumber,
    string DistributeAddress,
    DateTime AssignedAt,
    DateTime PromisedDeliveryAt,
    bool HasCOD,
    bool HasFMCG,
    int? FleetId,
    string? DriverName,
    DateTime? DeliveredAt,
    ReturnParcelReason? ReturnReason);

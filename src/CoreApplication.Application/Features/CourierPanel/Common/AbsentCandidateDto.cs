using CoreApplication.Domain.Enums;

namespace CoreApplication.Application.Features.CourierPanel.Common;

public record AbsentCandidateDto(
    int ParcelCourierId,
    int ParcelId,
    string Barcode,
    int LNumber,
    ServiceType ServiceType,
    string ReceiverFirstName,
    string ReceiverLastName,
    string ReceiverPhoneNumber,
    string DistributeAddress,
    bool HasCOD,
    DateTime AssignedAt,
    DateTime PromisedDeliveryAt,
    int DaysSinceAssignment);

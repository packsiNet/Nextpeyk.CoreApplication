using CoreApplication.Domain.Enums;

namespace CoreApplication.Application.Features.CourierPanel.Common;

public record ProcessingParcelDto(
    int ParcelCourierId,
    int ParcelId,
    string Barcode,
    int LNumber,
    ServiceType ServiceType,
    string ReceiverFirstName,
    string ReceiverLastName,
    string ReceiverPhoneNumber,
    double DistributeLatitude,
    double DistributeLongitude,
    string DistributeAddress,
    bool HasCOD,
    bool HasFMCG,
    bool HasFreightCollect,
    DateTime PromisedDeliveryAt);

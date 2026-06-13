using CoreApplication.Domain.Enums;

namespace CoreApplication.Application.Features.CourierPanel.Common;

public record ProcessingParcelDto(
    int ParcelCourierId,
    int ParcelId,
    string Barcode,
    int LNumber,
    ServiceType ServiceType,
    // Receiver info
    string ReceiverFirstName,
    string ReceiverLastName,
    string ReceiverPhoneNumber,
    // Distribution location (for map pin + list display)
    double DistributeLatitude,
    double DistributeLongitude,
    string DistributeAddress,
    // Collection location (Express: pickup point)
    double CollectLatitude,
    double CollectLongitude,
    string CollectAddress,
    // Parcel details
    bool HasCOD,
    bool HasFMCG,
    bool HasFreightCollect,
    decimal Weight,
    decimal Value,
    // Timing
    DateTime AssignedAt,
    DateTime PromisedDeliveryAt);

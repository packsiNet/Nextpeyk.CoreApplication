using CoreApplication.Domain.Enums;

namespace CoreApplication.Application.Features.FleetApp.Common;

public record AssignedParcelDto(
    int ParcelCourierFleetId,
    int ParcelCourierId,
    string Barcode,
    int LNumber,
    ServiceType ServiceType,
    ParcelTrackingEnum Status,
    int SortOrder,
    string DistributeFirstName,
    string DistributeLastName,
    string DistributePhoneNumber,
    double DistributeLatitude,
    double DistributeLongitude,
    string DistributeAddress,
    bool HasCOD,
    bool HasFMCG,
    bool HasFreightCollect,
    DateTime PromisedDeliveryAt);

using CoreApplication.Domain.Enums;

namespace CoreApplication.Application.Features.Parcels.Common;

public record ParcelCourierInfoDto(
    int CourierId,
    string CourierTitle,
    ServiceType ServiceType,
    ParcelTrackingEnum Status,
    decimal PickupCost,
    decimal DistributionCost,
    decimal TotalCost,
    DateTime AssignedAt,
    DateTime PromisedDeliveryAt);

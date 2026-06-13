using CoreApplication.Domain.Enums;

namespace CoreApplication.Application.Features.Parcels.Common;

public record ParcelTrackDto(
    int Id,
    string Barcode,
    ParcelState State,
    string? CourierTitle,
    ParcelTrackingEnum? TrackingStatus,
    DateTime AssignedAt,
    DateTime? PromisedDeliveryAt,
    DateTime? DeliveredAt,
    ReturnParcelReason? ReturnReason,
    string? ReturnNote);

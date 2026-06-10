using CoreApplication.Domain.Enums;

namespace CoreApplication.Application.Features.Parcels.Common;

public record ParcelListDto(
    int Id,
    string Barcode,
    ParcelState State,
    ParcelType ParcelType,
    string ReceiverFirstName,
    string ReceiverLastName,
    string ReceiverPhoneNumber,
    string ReceiverAddress,
    decimal Weight,
    bool HasCOD,
    DateTime CreatedAt);

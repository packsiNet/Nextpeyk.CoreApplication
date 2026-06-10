using CoreApplication.Domain.Enums;

namespace CoreApplication.Application.Features.Parcels.Common;

public record ParcelDetailDto(
    int Id,
    string Barcode,
    ParcelState State,
    ParcelType ParcelType,
    string? Description,
    decimal Length,
    decimal Width,
    decimal Height,
    decimal Weight,
    decimal Value,
    string SenderFirstName,
    string SenderLastName,
    string SenderPhoneNumber,
    decimal SenderLatitude,
    decimal SenderLongitude,
    string SenderAddress,
    string ReceiverFirstName,
    string ReceiverLastName,
    string ReceiverPhoneNumber,
    decimal ReceiverLatitude,
    decimal ReceiverLongitude,
    string ReceiverAddress,
    bool HasCOD,
    bool HasFMCG,
    bool HasFreightCollect,
    DateTime CreatedAt,
    ParcelCourierInfoDto? CourierInfo);

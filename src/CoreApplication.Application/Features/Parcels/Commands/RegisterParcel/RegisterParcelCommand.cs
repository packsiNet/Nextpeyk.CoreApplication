using CoreApplication.Domain.Enums;
using MediatR;

namespace CoreApplication.Application.Features.Parcels.Commands.RegisterParcel;

public record RegisterParcelCommand(
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
    bool HasFreightCollect) : IRequest<int>;

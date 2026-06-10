using MediatR;

namespace CoreApplication.Application.Features.Couriers.Commands.CreateCourier;

public record CreateCourierCommand(
    string Title,
    string? Description,
    string? Logo,
    string? SupportPhoneNumber,
    string? SupportFullName,
    string? Website,
    string? SystemName,
    string? EconomicCode,
    string? NationalCode,
    decimal BasePrice,
    decimal CodPrice,
    decimal FreightCollectPrice,
    int MaximumCapacityPerDay,
    decimal MaximumShipmentWeight,
    decimal MaximumValueOfTheShipment,
    int MinimumParcelsInOneOrder,
    bool HasCOD,
    bool HasFMCG,
    bool HasFreightCollect,
    bool HasPackaging,
    bool IsGroupAcceptance) : IRequest<int>;

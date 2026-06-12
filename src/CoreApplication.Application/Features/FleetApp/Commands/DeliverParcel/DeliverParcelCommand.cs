using MediatR;

namespace CoreApplication.Application.Features.FleetApp.Commands.DeliverParcel;

public record DeliverParcelCommand(
    int ParcelCourierFleetId,
    string ReceiverName,
    int? AuthType,
    string? Signature,
    string? PODCode) : IRequest;

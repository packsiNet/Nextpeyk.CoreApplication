using CoreApplication.Domain.Enums;
using MediatR;

namespace CoreApplication.Application.Features.FleetApp.Commands.ReturnParcel;

public record ReturnParcelCommand(
    int ParcelCourierFleetId,
    ReturnParcelReason ReturnReason,
    string? ReturnNote,
    List<string> ImagePaths) : IRequest;

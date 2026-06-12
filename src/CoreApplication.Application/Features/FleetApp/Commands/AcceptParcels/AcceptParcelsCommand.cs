using MediatR;

namespace CoreApplication.Application.Features.FleetApp.Commands.AcceptParcels;

/// <summary>
/// Accept one or more assigned parcels.
/// Single mode: pass one Id. Group mode: pass all Ids.
/// Mode (single/group) enforced by Flutter app based on CourierSetting.IsGroupAcceptance.
/// </summary>
public record AcceptParcelsCommand(List<int> ParcelCourierFleetIds) : IRequest;

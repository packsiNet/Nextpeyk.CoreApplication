using CoreApplication.Application.Features.FleetApp.Commands.AcceptParcels;
using CoreApplication.Application.Features.FleetApp.Commands.DeliverParcel;
using CoreApplication.Application.Features.FleetApp.Commands.ReturnParcel;
using CoreApplication.Application.Features.FleetApp.Queries.GetAssignedParcels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreApplication.API.Controllers;

[Authorize(Roles = "Driver")]
public class FleetAppController : ApiControllerBase
{
    /// <summary>GET parcels assigned or accepted by this driver today.</summary>
    [HttpGet("parcels")]
    public async Task<IActionResult> GetParcels(CancellationToken ct)
        => Ok(await Mediator.Send(new GetAssignedParcelsQuery(), ct));

    /// <summary>
    /// Accept one or more parcels (AssignToFleet → ReceivedByFleet).
    /// Single mode: one Id. Group mode: all Ids at once.
    /// The Flutter app decides based on CourierSetting.IsGroupAcceptance.
    /// </summary>
    [HttpPost("parcels/accept")]
    public async Task<IActionResult> Accept([FromBody] AcceptParcelsCommand command, CancellationToken ct)
    {
        await Mediator.Send(command, ct);
        return NoContent();
    }

    /// <summary>Mark parcel as delivered + record POD (ReceivedByFleet → FinalizedDelivery).</summary>
    [HttpPost("parcels/{id:int}/deliver")]
    public async Task<IActionResult> Deliver(int id, [FromBody] DeliverParcelRequest request, CancellationToken ct)
    {
        await Mediator.Send(new DeliverParcelCommand(id, request.ReceiverName, request.AuthType, request.Signature, request.PODCode), ct);
        return NoContent();
    }

    /// <summary>Mark parcel as returned + reason + images (ReceivedByFleet → Returned).</summary>
    [HttpPost("parcels/{id:int}/return")]
    public async Task<IActionResult> Return(int id, [FromBody] ReturnParcelRequest request, CancellationToken ct)
    {
        await Mediator.Send(new ReturnParcelCommand(id, request.ReturnReason, request.ReturnNote, request.ImagePaths), ct);
        return NoContent();
    }
}

public record DeliverParcelRequest(
    string ReceiverName,
    int? AuthType,
    string? Signature,
    string? PODCode);

public record ReturnParcelRequest(
    Domain.Enums.ReturnParcelReason ReturnReason,
    string? ReturnNote,
    List<string> ImagePaths);

using CoreApplication.Application.Features.CourierPanel.Commands.AssignToFleet;
using CoreApplication.Application.Features.CourierPanel.Commands.ReassignParcel;
using CoreApplication.Application.Features.CourierPanel.Commands.SettleFleet;
using CoreApplication.Application.Features.CourierPanel.Queries.GetProcessingParcels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreApplication.API.Controllers;

[Authorize(Roles = "CourierManager,CourierOperator")]
public class CourierPanelController : ApiControllerBase
{
    [HttpGet("parcels/processing")]
    public async Task<IActionResult> GetProcessingParcels(CancellationToken ct)
        => Ok(await Mediator.Send(new GetProcessingParcelsQuery(), ct));

    [HttpPost("parcels/assign-to-fleet")]
    [Authorize(Roles = "CourierManager")]
    public async Task<IActionResult> AssignToFleet([FromBody] AssignToFleetCommand command, CancellationToken ct)
    {
        await Mediator.Send(command, ct);
        return NoContent();
    }

    /// <summary>
    /// Cancel fleet assignment — only before driver accepts (AssignToFleet status).
    /// Parcel returns to Processing and reappears on the map.
    /// </summary>
    [HttpPost("parcels/{id:int}/reassign")]
    [Authorize(Roles = "CourierManager")]
    public async Task<IActionResult> Reassign(int id, CancellationToken ct)
    {
        await Mediator.Send(new ReassignParcelCommand(id), ct);
        return NoContent();
    }

    /// <summary>
    /// Confirm daily settlement for a fleet driver.
    /// Marks all finalized/returned parcels for the given date as settled.
    /// Returns count of settled records.
    /// </summary>
    [HttpPost("fleets/{fleetId:int}/settle")]
    [Authorize(Roles = "CourierManager")]
    public async Task<IActionResult> Settle(int fleetId, [FromQuery] DateOnly? date, CancellationToken ct)
    {
        var count = await Mediator.Send(new SettleFleetCommand(fleetId, date), ct);
        return Ok(new { settledCount = count });
    }
}

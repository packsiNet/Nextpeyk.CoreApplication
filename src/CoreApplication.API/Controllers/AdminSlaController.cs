using CoreApplication.Application.Features.Sla.Commands.MarkSlaAlertRead;
using CoreApplication.Application.Features.Sla.Commands.UpdateSlaGlobalConfig;
using CoreApplication.Application.Features.Sla.Queries.GetSlaAlerts;
using CoreApplication.Application.Features.Sla.Queries.GetSlaGlobalConfig;
using CoreApplication.Application.Features.Sla.Queries.GetSlaSnapshots;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreApplication.API.Controllers;

[Authorize(Roles = "Admin")]
[Route("api/admin/sla")]
public class AdminSlaController : ApiControllerBase
{
    // --- SLA Global Config ---

    [HttpGet("config")]
    public async Task<IActionResult> GetConfig(CancellationToken ct)
        => Ok(await Mediator.Send(new GetSlaGlobalConfigQuery(), ct));

    [HttpPut("config")]
    public async Task<IActionResult> UpdateConfig([FromBody] UpdateSlaGlobalConfigCommand command, CancellationToken ct)
    {
        var id = await Mediator.Send(command, ct);
        return Ok(new { id });
    }

    // --- SLA Snapshots ---

    [HttpGet("snapshots")]
    public async Task<IActionResult> GetSnapshots([FromQuery] GetSlaSnapshotsQuery query, CancellationToken ct)
        => Ok(await Mediator.Send(query, ct));

    // --- SLA Alerts ---

    [HttpGet("alerts")]
    public async Task<IActionResult> GetAlerts([FromQuery] GetSlaAlertsQuery query, CancellationToken ct)
        => Ok(await Mediator.Send(query, ct));

    [HttpPut("alerts/{id:int}/read")]
    public async Task<IActionResult> MarkAlertRead(int id, CancellationToken ct)
    {
        await Mediator.Send(new MarkSlaAlertReadCommand(id), ct);
        return NoContent();
    }
}

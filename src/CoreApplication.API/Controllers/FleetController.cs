using CoreApplication.Application.Features.Fleets.Commands.CreateFleet;
using CoreApplication.Application.Features.Fleets.Commands.CreateFleetType;
using CoreApplication.Application.Features.Fleets.Commands.DeleteFleet;
using CoreApplication.Application.Features.Fleets.Commands.DeleteFleetType;
using CoreApplication.Application.Features.Fleets.Commands.UpdateFleet;
using CoreApplication.Application.Features.Fleets.Queries.GetFleetById;
using CoreApplication.Application.Features.Fleets.Queries.GetFleets;
using CoreApplication.Application.Features.Fleets.Queries.GetFleetTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreApplication.API.Controllers;

public class FleetController : ApiControllerBase
{
    // ─── FleetType (Admin) ───────────────────────────────────────────────────

    [HttpGet("types")]
    [Authorize(Roles = "Admin,CourierManager,CourierOperator")]
    public async Task<IActionResult> GetFleetTypes(CancellationToken ct)
        => Ok(await Mediator.Send(new GetFleetTypesQuery(), ct));

    [HttpPost("types")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateFleetType([FromBody] CreateFleetTypeCommand command, CancellationToken ct)
    {
        var id = await Mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetFleetTypes), new { }, new { id });
    }

    [HttpDelete("types/{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteFleetType(int id, CancellationToken ct)
    {
        await Mediator.Send(new DeleteFleetTypeCommand(id), ct);
        return NoContent();
    }

    // ─── Fleet (CourierManager) ──────────────────────────────────────────────

    [HttpGet]
    [Authorize(Roles = "CourierManager,CourierOperator")]
    public async Task<IActionResult> GetAll([FromQuery] bool? activeOnly, CancellationToken ct)
        => Ok(await Mediator.Send(new GetFleetsQuery(activeOnly), ct));

    [HttpGet("{id:int}")]
    [Authorize(Roles = "CourierManager,CourierOperator")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
        => Ok(await Mediator.Send(new GetFleetByIdQuery(id), ct));

    [HttpPost]
    [Authorize(Roles = "CourierManager")]
    public async Task<IActionResult> Create([FromBody] CreateFleetCommand command, CancellationToken ct)
    {
        var id = await Mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "CourierManager")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateFleetRequest request, CancellationToken ct)
    {
        await Mediator.Send(new UpdateFleetCommand(
            id,
            request.FleetTypeId,
            request.Plaque,
            request.DrivingLicense,
            request.StartDate,
            request.EndDate,
            request.Description,
            request.InsuranceCode,
            request.IsActive), ct);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "CourierManager")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await Mediator.Send(new DeleteFleetCommand(id), ct);
        return NoContent();
    }
}

public record UpdateFleetRequest(
    int FleetTypeId,
    string Plaque,
    string DrivingLicense,
    DateOnly StartDate,
    DateOnly? EndDate,
    string? Description,
    string? InsuranceCode,
    bool IsActive);

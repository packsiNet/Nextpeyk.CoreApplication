using CoreApplication.Application.Features.Parcels.Commands.ConfirmParcel;
using CoreApplication.Application.Features.Parcels.Commands.RegisterParcel;
using CoreApplication.Application.Features.Parcels.Queries.GetParcelById;
using CoreApplication.Application.Features.Parcels.Queries.GetParcels;
using CoreApplication.Application.Features.Parcels.Queries.InquireParcel;
using CoreApplication.Application.Features.Parcels.Queries.TrackParcel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreApplication.API.Controllers;

[Authorize(Roles = "Sender")]
public class ParcelController : ApiControllerBase
{
    [HttpGet("inquire")]
    public async Task<IActionResult> Inquire([FromQuery] InquireParcelQuery query, CancellationToken ct)
        => Ok(await Mediator.Send(query, ct));

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetParcelsQuery query, CancellationToken ct)
        => Ok(await Mediator.Send(query, ct));

    [HttpGet("track/{barcode}")]
    public async Task<IActionResult> Track(string barcode, CancellationToken ct)
        => Ok(await Mediator.Send(new TrackParcelQuery(barcode), ct));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
        => Ok(await Mediator.Send(new GetParcelByIdQuery(id), ct));

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterParcelCommand command, CancellationToken ct)
    {
        var parcelId = await Mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = parcelId }, new { parcelId });
    }

    [HttpPost("{id:int}/confirm")]
    public async Task<IActionResult> Confirm(int id, [FromBody] ConfirmParcelRequest request, CancellationToken ct)
    {
        await Mediator.Send(new ConfirmParcelCommand(id, request.CourierId), ct);
        return NoContent();
    }
}

public record ConfirmParcelRequest(int CourierId);

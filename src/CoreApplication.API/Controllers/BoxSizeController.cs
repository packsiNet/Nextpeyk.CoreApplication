using CoreApplication.Application.Features.Couriers.Commands.CreateBoxSize;
using CoreApplication.Application.Features.Couriers.Commands.UpdateBoxSize;
using CoreApplication.Application.Features.Couriers.Queries.GetBoxSizes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreApplication.API.Controllers;

[Authorize(Roles = "Admin")]
public class BoxSizeController : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await Mediator.Send(new GetBoxSizesQuery(), ct));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBoxSizeCommand command, CancellationToken ct)
    {
        var id = await Mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetAll), new { id }, new { id });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateBoxSizeRequest request, CancellationToken ct)
    {
        await Mediator.Send(new UpdateBoxSizeCommand(id, request.Order, request.Title, request.Code,
            request.BoxLength, request.BoxWidth, request.BoxHeight), ct);
        return NoContent();
    }
}

public record UpdateBoxSizeRequest(
    int Order,
    string Title,
    string Code,
    decimal BoxLength,
    decimal BoxWidth,
    decimal BoxHeight);

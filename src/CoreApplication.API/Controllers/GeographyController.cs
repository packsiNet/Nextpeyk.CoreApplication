using CoreApplication.Application.Features.Geography.Commands.CreateCity;
using CoreApplication.Application.Features.Geography.Commands.DeleteCity;
using CoreApplication.Application.Features.Geography.Commands.UpdateCity;
using CoreApplication.Application.Features.Geography.Queries.GetCities;
using CoreApplication.Application.Features.Geography.Queries.GetCityById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreApplication.API.Controllers;

[Authorize(Roles = "Admin")]
public class GeographyController : ApiControllerBase
{
    [HttpGet("cities")]
    public async Task<IActionResult> GetCities([FromQuery] GetCitiesQuery query, CancellationToken ct)
        => Ok(await Mediator.Send(query, ct));

    [HttpGet("cities/{id:int}")]
    public async Task<IActionResult> GetCity(int id, CancellationToken ct)
        => Ok(await Mediator.Send(new GetCityByIdQuery(id), ct));

    [HttpPost("cities")]
    public async Task<IActionResult> CreateCity([FromBody] CreateCityCommand command, CancellationToken ct)
    {
        var cityId = await Mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetCity), new { id = cityId }, new { cityId });
    }

    [HttpPut("cities/{id:int}")]
    public async Task<IActionResult> UpdateCity(int id, [FromBody] UpdateCityRequest request, CancellationToken ct)
    {
        await Mediator.Send(new UpdateCityCommand(id, request.Name, request.Boundary), ct);
        return NoContent();
    }

    [HttpDelete("cities/{id:int}")]
    public async Task<IActionResult> DeleteCity(int id, CancellationToken ct)
    {
        await Mediator.Send(new DeleteCityCommand(id), ct);
        return NoContent();
    }
}

public record UpdateCityRequest(string Name, string? Boundary);

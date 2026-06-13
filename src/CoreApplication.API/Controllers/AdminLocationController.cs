using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Application.Features.Admin.Queries.GetActiveDriverLocations;
using CoreApplication.Application.Features.Admin.Queries.GetCourierTrack;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreApplication.API.Controllers;

[Authorize(Roles = "Admin,CourierManager,CourierOperator")]
[Route("api/location")]
public class AdminLocationController(ICurrentUserService currentUser) : ApiControllerBase
{
    /// <summary>
    /// Active drivers with last known position (today).
    /// Admin: all couriers (optional filter by courierId).
    /// CourierManager/Operator: only their own courier's drivers.
    /// </summary>
    [HttpGet("active")]
    public async Task<IActionResult> GetActive([FromQuery] int? courierId, CancellationToken ct)
    {
        var effectiveCourierId = currentUser.IsInRole("Admin") ? courierId : currentUser.CourierId;
        return Ok(await Mediator.Send(new GetActiveDriverLocationsQuery(effectiveCourierId), ct));
    }

    /// <summary>
    /// Full route history for a fleet driver on a given date.
    /// CourierManager/Operator can only query fleets belonging to their courier.
    /// </summary>
    [HttpGet("track/{fleetId:int}")]
    public async Task<IActionResult> GetTrack(
        int fleetId,
        [FromQuery] DateOnly date,
        [FromQuery] bool simplify = false,
        [FromQuery] bool withMapMatching = false,
        CancellationToken ct = default)
    {
        var requireCourierId = currentUser.IsInRole("Admin") ? null : currentUser.CourierId;
        return Ok(await Mediator.Send(
            new GetCourierTrackQuery(fleetId, date, simplify, withMapMatching, requireCourierId), ct));
    }
}

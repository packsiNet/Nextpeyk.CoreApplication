using CoreApplication.Application.Features.Admin.Queries.GetAdminDashboardStats;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreApplication.API.Controllers;

[Authorize(Roles = "Admin")]
[Route("api/admin/dashboard")]
public class AdminDashboardController : ApiControllerBase
{
    [HttpGet("stats")]
    public async Task<IActionResult> GetStats(CancellationToken ct)
        => Ok(await Mediator.Send(new GetAdminDashboardStatsQuery(), ct));
}

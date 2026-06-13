using CoreApplication.Application.Features.Admin.Reports.Queries.GetActiveAlertsSummary;
using CoreApplication.Application.Features.Admin.Reports.Queries.GetCourierCapacity;
using CoreApplication.Application.Features.Admin.Reports.Queries.GetCourierFinancial;
using CoreApplication.Application.Features.Admin.Reports.Queries.GetCourierRanking;
using CoreApplication.Application.Features.Admin.Reports.Queries.GetCourierTrend;
using CoreApplication.Application.Features.Admin.Reports.Queries.GetReturnReasonBreakdown;
using CoreApplication.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreApplication.API.Controllers;

[Authorize(Roles = "Admin")]
[Route("api/admin/reports")]
public class AdminReportController : ApiControllerBase
{
    // GET api/admin/reports/couriers/ranking?periodType=Rolling30
    [HttpGet("couriers/ranking")]
    public async Task<IActionResult> GetCourierRanking(
        [FromQuery] SlaPeriodType periodType = SlaPeriodType.Rolling30,
        CancellationToken ct = default)
        => Ok(await Mediator.Send(new GetCourierRankingQuery(periodType), ct));

    // GET api/admin/reports/couriers/{id}/trend?periodType=Weekly&lastN=12
    [HttpGet("couriers/{id:int}/trend")]
    public async Task<IActionResult> GetCourierTrend(
        int id,
        [FromQuery] SlaPeriodType periodType = SlaPeriodType.Weekly,
        [FromQuery] int lastN = 12,
        CancellationToken ct = default)
        => Ok(await Mediator.Send(new GetCourierTrendQuery(id, periodType, lastN), ct));

    // GET api/admin/reports/couriers/return-reasons?courierId=&from=&to=
    [HttpGet("couriers/return-reasons")]
    public async Task<IActionResult> GetReturnReasons(
        [FromQuery] int? courierId = null,
        [FromQuery] DateOnly? from = null,
        [FromQuery] DateOnly? to = null,
        CancellationToken ct = default)
        => Ok(await Mediator.Send(new GetReturnReasonBreakdownQuery(courierId, from, to), ct));

    // GET api/admin/reports/couriers/capacity?from=&to=
    [HttpGet("couriers/capacity")]
    public async Task<IActionResult> GetCapacity(
        [FromQuery] DateOnly? from = null,
        [FromQuery] DateOnly? to = null,
        CancellationToken ct = default)
        => Ok(await Mediator.Send(new GetCourierCapacityQuery(from, to), ct));

    // GET api/admin/reports/couriers/financial?courierId=&from=&to=
    [HttpGet("couriers/financial")]
    public async Task<IActionResult> GetFinancial(
        [FromQuery] int? courierId = null,
        [FromQuery] DateOnly? from = null,
        [FromQuery] DateOnly? to = null,
        CancellationToken ct = default)
        => Ok(await Mediator.Send(new GetCourierFinancialQuery(courierId, from, to), ct));

    // GET api/admin/reports/sla/active-alerts
    [HttpGet("sla/active-alerts")]
    public async Task<IActionResult> GetActiveAlerts(CancellationToken ct = default)
        => Ok(await Mediator.Send(new GetActiveAlertsSummaryQuery(), ct));
}

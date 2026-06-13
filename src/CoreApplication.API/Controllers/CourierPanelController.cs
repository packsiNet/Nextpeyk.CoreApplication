using CoreApplication.Application.Features.CourierPanel.Commands.AssignToFleet;
using CoreApplication.Application.Features.CourierPanel.Commands.CancelFleetAssignments;
using CoreApplication.Application.Features.CourierPanel.Commands.ConfirmAbsentParcels;
using CoreApplication.Application.Features.CourierPanel.Commands.ReassignParcel;
using CoreApplication.Application.Features.CourierPanel.Commands.RevertAbsentParcel;
using CoreApplication.Application.Features.CourierPanel.Commands.SettleFleet;
using CoreApplication.Application.Features.CourierPanel.Queries.GetAbsentCandidates;
using CoreApplication.Application.Features.CourierPanel.Queries.GetCourierSummaryReport;
using CoreApplication.Application.Features.CourierPanel.Queries.GetDriverSettlementReport;
using CoreApplication.Application.Features.CourierPanel.Queries.GetCourierDashboard;
using CoreApplication.Application.Features.CourierPanel.Queries.GetCourierParcels;
using CoreApplication.Application.Features.CourierPanel.Queries.GetCourierSlaTrend;
using CoreApplication.Application.Features.CourierPanel.Queries.GetFleetDailyReport;
using CoreApplication.Application.Features.CourierPanel.Queries.GetFleetUnsettled;
using CoreApplication.Application.Features.CourierPanel.Queries.GetProcessingParcels;
using CoreApplication.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreApplication.API.Controllers;

[Authorize(Roles = "CourierManager,CourierOperator")]
public class CourierPanelController : ApiControllerBase
{
    // --- Core Operations ---

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

    [HttpPost("parcels/{id:int}/reassign")]
    [Authorize(Roles = "CourierManager")]
    public async Task<IActionResult> Reassign(int id, CancellationToken ct)
    {
        await Mediator.Send(new ReassignParcelCommand(id), ct);
        return NoContent();
    }

    [HttpPost("fleets/{fleetId:int}/settle")]
    [Authorize(Roles = "CourierManager")]
    public async Task<IActionResult> Settle(int fleetId, [FromQuery] DateOnly? date, CancellationToken ct)
    {
        var count = await Mediator.Send(new SettleFleetCommand(fleetId, date), ct);
        return Ok(new { settledCount = count });
    }

    /// <summary>
    /// لغو تمام تخصیص‌های امروز یک ناوگان — فقط مرسولاتی که راننده هنوز نپذیرفته (AssignToFleet)
    /// </summary>
    [HttpPost("fleets/{fleetId:int}/cancel-assignments")]
    [Authorize(Roles = "CourierManager")]
    public async Task<IActionResult> CancelFleetAssignments(int fleetId, CancellationToken ct)
    {
        var count = await Mediator.Send(new CancelFleetAssignmentsCommand(fleetId), ct);
        return Ok(new { cancelledCount = count });
    }

    // --- Physical Absence Management ---

    /// <summary>
    /// لیست مرسولاتی که از روزهای قبل در Processing مانده‌اند — کاندیدای عدم فیزیک
    /// </summary>
    [HttpGet("parcels/absent-candidates")]
    public async Task<IActionResult> GetAbsentCandidates(CancellationToken ct)
        => Ok(await Mediator.Send(new GetAbsentCandidatesQuery(), ct));

    /// <summary>
    /// تأیید bulk عدم فیزیک — مرسولات از Processing به PhysicallyAbsent می‌روند
    /// </summary>
    [HttpPost("parcels/confirm-absent")]
    [Authorize(Roles = "CourierManager")]
    public async Task<IActionResult> ConfirmAbsent(
        [FromBody] ConfirmAbsentParcelsCommand command,
        CancellationToken ct)
    {
        var count = await Mediator.Send(command, ct);
        return Ok(new { confirmedCount = count });
    }

    /// <summary>
    /// برگشت مرسوله عدم به Processing — وقتی فیزیک مرسوله رسید
    /// </summary>
    [HttpPost("parcels/{id:int}/revert-absent")]
    [Authorize(Roles = "CourierManager")]
    public async Task<IActionResult> RevertAbsent(int id, CancellationToken ct)
    {
        await Mediator.Send(new RevertAbsentParcelCommand(id), ct);
        return NoContent();
    }

    // --- Summary Report ---

    /// <summary>
    /// گزارش مالی روزانه راننده‌ها: پیشرفت تحویل، COD کل و تحویل‌داده‌شده، وضعیت تسویه
    /// </summary>
    [HttpGet("reports/driver-settlement")]
    public async Task<IActionResult> GetDriverSettlementReport(
        [FromQuery] DateOnly date,
        CancellationToken ct)
        => Ok(await Mediator.Send(new GetDriverSettlementReportQuery(date), ct));

    /// <summary>
    /// گزارش خلاصه: کل، عدم، تایید شده، مرجوعی مجاز/نامجاز، تحویل داده شده
    /// مرجوعی مجاز = دارای عکس | نامجاز = بدون عکس
    /// </summary>
    [HttpGet("reports/summary")]
    public async Task<IActionResult> GetSummaryReport(
        [FromQuery] DateOnly from,
        [FromQuery] DateOnly to,
        CancellationToken ct)
        => Ok(await Mediator.Send(new GetCourierSummaryReportQuery(from, to), ct));

    // --- Dashboard & Reports ---

    /// <summary>
    /// خلاصه وضعیت امروز: تعداد مرسولات در هر مرحله + ناوگان فعال + موارد تسویه‌نشده
    /// </summary>
    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard(CancellationToken ct)
        => Ok(await Mediator.Send(new GetCourierDashboardQuery(), ct));

    /// <summary>
    /// لیست مرسولات با فیلتر وضعیت، ناوگان و بازه تاریخی — صفحه‌بندی شده
    /// </summary>
    [HttpGet("parcels")]
    public async Task<IActionResult> GetParcels(
        [FromQuery] ParcelTrackingEnum? status = null,
        [FromQuery] int? fleetId = null,
        [FromQuery] DateOnly? from = null,
        [FromQuery] DateOnly? to = null,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
        => Ok(await Mediator.Send(
            new GetCourierParcelsQuery(status, fleetId, from, to, pageNumber, pageSize), ct));

    /// <summary>
    /// گزارش روزانه هر راننده: تحویل، مرجوع، معلق، تسویه‌نشده
    /// </summary>
    [HttpGet("fleets/daily-report")]
    public async Task<IActionResult> GetFleetDailyReport(
        [FromQuery] DateOnly? date = null,
        CancellationToken ct = default)
        => Ok(await Mediator.Send(new GetFleetDailyReportQuery(date), ct));

    /// <summary>
    /// روند SLA این Courier در N دوره گذشته
    /// </summary>
    [HttpGet("sla/trend")]
    public async Task<IActionResult> GetSlaTrend(
        [FromQuery] SlaPeriodType periodType = SlaPeriodType.Weekly,
        [FromQuery] int lastN = 12,
        CancellationToken ct = default)
        => Ok(await Mediator.Send(new GetCourierSlaTrendQuery(periodType, lastN), ct));

    /// <summary>
    /// مرسولات تسویه‌نشده یک راننده — اختیاری: فیلتر روز
    /// </summary>
    [HttpGet("fleets/{fleetId:int}/unsettled")]
    public async Task<IActionResult> GetFleetUnsettled(
        int fleetId,
        [FromQuery] DateOnly? date = null,
        CancellationToken ct = default)
        => Ok(await Mediator.Send(new GetFleetUnsettledQuery(fleetId, date), ct));
}

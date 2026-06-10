using CoreApplication.Application.Features.Couriers.Commands.AddCourierZone;
using CoreApplication.Application.Features.Couriers.Commands.AssignBoxSizeToCourier;
using CoreApplication.Application.Features.Couriers.Commands.CreateCourier;
using CoreApplication.Application.Features.Couriers.Commands.DeleteCourier;
using CoreApplication.Application.Features.Couriers.Commands.DeleteCourierZone;
using CoreApplication.Application.Features.Couriers.Commands.RemoveBoxSizeFromCourier;
using CoreApplication.Application.Features.Couriers.Commands.SetCourierActivityTimes;
using CoreApplication.Application.Features.Couriers.Commands.SetCourierSlaConfigs;
using CoreApplication.Application.Features.Couriers.Commands.UpdateCourier;
using CoreApplication.Application.Features.Couriers.Commands.UpdateCourierZone;
using CoreApplication.Application.Features.Couriers.Queries.GetCourierById;
using CoreApplication.Application.Features.Couriers.Queries.GetCouriers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetTopologySuite.Geometries;

namespace CoreApplication.API.Controllers;

[Authorize(Roles = "Admin")]
public class CourierController : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetCouriersQuery query, CancellationToken ct)
        => Ok(await Mediator.Send(query, ct));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
        => Ok(await Mediator.Send(new GetCourierByIdQuery(id), ct));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCourierCommand command, CancellationToken ct)
    {
        var courierId = await Mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = courierId }, new { courierId });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCourierRequest request, CancellationToken ct)
    {
        await Mediator.Send(new UpdateCourierCommand(
            id, request.Title, request.Description, request.Logo,
            request.SupportPhoneNumber, request.SupportFullName, request.Website,
            request.SystemName, request.EconomicCode, request.NationalCode,
            request.BasePrice, request.CodPrice, request.FreightCollectPrice,
            request.MaximumCapacityPerDay, request.MaximumShipmentWeight,
            request.MaximumValueOfTheShipment, request.MinimumParcelsInOneOrder,
            request.HasCOD, request.HasFMCG, request.HasFreightCollect,
            request.HasPackaging, request.IsGroupAcceptance), ct);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await Mediator.Send(new DeleteCourierCommand(id), ct);
        return NoContent();
    }

    // --- Zones ---

    [HttpPost("{courierId:int}/zones")]
    public async Task<IActionResult> AddZone(int courierId, [FromBody] AddZoneRequest request, CancellationToken ct)
    {
        var zoneId = await Mediator.Send(new AddCourierZoneCommand(
            courierId, request.ZoneType, request.Name, request.CityId,
            request.Boundary, request.CenterPoint, request.Radius, request.Price), ct);
        return CreatedAtAction(nameof(GetById), new { id = courierId }, new { zoneId });
    }

    [HttpPut("{courierId:int}/zones/{zoneId:int}")]
    public async Task<IActionResult> UpdateZone(int zoneId, [FromBody] UpdateZoneRequest request, CancellationToken ct)
    {
        await Mediator.Send(new UpdateCourierZoneCommand(
            zoneId, request.ZoneType, request.Name, request.CityId,
            request.Boundary, request.CenterPoint, request.Radius, request.Price), ct);
        return NoContent();
    }

    [HttpDelete("{courierId:int}/zones/{zoneId:int}")]
    public async Task<IActionResult> DeleteZone(int zoneId, CancellationToken ct)
    {
        await Mediator.Send(new DeleteCourierZoneCommand(zoneId), ct);
        return NoContent();
    }

    // --- Activity Times ---

    [HttpPut("{courierId:int}/activity-times")]
    public async Task<IActionResult> SetActivityTimes(int courierId, [FromBody] List<ActivityTimeItem> items, CancellationToken ct)
    {
        await Mediator.Send(new SetCourierActivityTimesCommand(courierId, items), ct);
        return NoContent();
    }

    // --- SLA Configs ---

    [HttpPut("{courierId:int}/sla-configs")]
    public async Task<IActionResult> SetSlaConfigs(int courierId, [FromBody] List<SlaConfigItem> items, CancellationToken ct)
    {
        await Mediator.Send(new SetCourierSlaConfigsCommand(courierId, items), ct);
        return NoContent();
    }

    // --- Box Sizes ---

    [HttpPost("{courierId:int}/box-sizes")]
    public async Task<IActionResult> AssignBoxSize(int courierId, [FromBody] AssignBoxSizeRequest request, CancellationToken ct)
    {
        var id = await Mediator.Send(new AssignBoxSizeToCourierCommand(courierId, request.BoxSizeId, request.PackagingPrice), ct);
        return CreatedAtAction(nameof(GetById), new { id = courierId }, new { id });
    }

    [HttpDelete("{courierId:int}/box-sizes/{courierBoxSizeId:int}")]
    public async Task<IActionResult> RemoveBoxSize(int courierBoxSizeId, CancellationToken ct)
    {
        await Mediator.Send(new RemoveBoxSizeFromCourierCommand(courierBoxSizeId), ct);
        return NoContent();
    }
}

public record UpdateCourierRequest(
    string Title, string? Description, string? Logo,
    string? SupportPhoneNumber, string? SupportFullName, string? Website,
    string? SystemName, string? EconomicCode, string? NationalCode,
    decimal BasePrice, decimal CodPrice, decimal FreightCollectPrice,
    int MaximumCapacityPerDay, decimal MaximumShipmentWeight,
    decimal MaximumValueOfTheShipment, int MinimumParcelsInOneOrder,
    bool HasCOD, bool HasFMCG, bool HasFreightCollect,
    bool HasPackaging, bool IsGroupAcceptance);

public record AddZoneRequest(
    CoreApplication.Domain.Enums.ZoneType ZoneType,
    string Name, int? CityId,
    Geometry? Boundary,
    Point? CenterPoint,
    decimal Radius, decimal? Price);

public record UpdateZoneRequest(
    CoreApplication.Domain.Enums.ZoneType ZoneType,
    string Name, int? CityId,
    Geometry? Boundary,
    Point? CenterPoint,
    decimal Radius, decimal? Price);

public record AssignBoxSizeRequest(int BoxSizeId, decimal PackagingPrice);

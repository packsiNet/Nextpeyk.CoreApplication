using MediatR;

namespace CoreApplication.Application.Features.CourierPanel.Commands.SettleFleet;

/// <summary>
/// Confirm daily settlement for a fleet driver.
/// Marks all finalized/returned parcels for the given date as settled.
/// </summary>
public record SettleFleetCommand(int FleetId, DateOnly? Date = null) : IRequest<int>;

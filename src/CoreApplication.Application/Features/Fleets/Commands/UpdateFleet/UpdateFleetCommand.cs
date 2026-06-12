using MediatR;

namespace CoreApplication.Application.Features.Fleets.Commands.UpdateFleet;

public record UpdateFleetCommand(
    int Id,
    int FleetTypeId,
    string Plaque,
    string DrivingLicense,
    DateOnly StartDate,
    DateOnly? EndDate,
    string? Description,
    string? InsuranceCode,
    bool IsActive) : IRequest;

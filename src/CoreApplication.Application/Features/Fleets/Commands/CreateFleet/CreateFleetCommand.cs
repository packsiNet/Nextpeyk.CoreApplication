using MediatR;

namespace CoreApplication.Application.Features.Fleets.Commands.CreateFleet;

public record CreateFleetCommand(
    // Driver account
    string UserName,
    string Password,
    string PhoneNumber,
    string FirstName,
    string LastName,
    string? NationalCode,
    // Fleet info
    int FleetTypeId,
    string Plaque,
    string DrivingLicense,
    DateOnly StartDate,
    DateOnly? EndDate,
    string? Description,
    string? InsuranceCode) : IRequest<int>;

namespace CoreApplication.Application.Features.Fleets.Common;

public record FleetListDto(
    int Id,
    int UserAccountId,
    string? DriverFirstName,
    string? DriverLastName,
    string? DriverPhoneNumber,
    int FleetTypeId,
    string FleetTypeTitle,
    string Plaque,
    DateOnly StartDate,
    DateOnly? EndDate,
    bool IsActive);

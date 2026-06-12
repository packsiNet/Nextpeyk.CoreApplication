namespace CoreApplication.Application.Features.Fleets.Common;

public record FleetDetailDto(
    int Id,
    int UserAccountId,
    string? DriverFirstName,
    string? DriverLastName,
    string? DriverPhoneNumber,
    int CourierId,
    int FleetTypeId,
    string FleetTypeTitle,
    string Plaque,
    string DrivingLicense,
    DateOnly StartDate,
    DateOnly? EndDate,
    string? Description,
    string? ProfileImage,
    string? InsuranceCode,
    bool IsActive);

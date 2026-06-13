namespace CoreApplication.Application.Features.CourierPanel.Common;

public record FleetDailyReportDto(
    int FleetId,
    string Plaque,
    string DriverFirstName,
    string DriverLastName,
    int TotalAssigned,
    int Delivered,
    int Returned,
    int Pending,
    int Unsettled,
    bool HasCODParcels);

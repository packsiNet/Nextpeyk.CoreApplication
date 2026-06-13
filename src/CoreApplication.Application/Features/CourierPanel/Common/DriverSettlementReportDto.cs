namespace CoreApplication.Application.Features.CourierPanel.Common;

public record DriverSettlementReportDto(
    int FleetId,
    string Plaque,
    string DriverFirstName,
    string DriverLastName,
    int TotalAccepted,
    int Delivered,
    int Returned,
    int InProgress,
    decimal ProgressPercent,
    decimal TotalCodAmount,
    decimal DeliveredCodAmount,
    int SettledCount,
    bool IsFullySettled);

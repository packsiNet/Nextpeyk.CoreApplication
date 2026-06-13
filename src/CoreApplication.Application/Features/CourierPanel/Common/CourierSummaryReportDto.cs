namespace CoreApplication.Application.Features.CourierPanel.Common;

public record CourierSummaryReportDto(
    DateOnly From,
    DateOnly To,
    int Total,
    int PhysicallyAbsent,
    int Confirmed,
    int TotalReturned,
    int ValidReturns,
    int InvalidReturns,
    int Delivered);

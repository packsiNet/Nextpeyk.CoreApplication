namespace CoreApplication.Application.Features.Admin.Reports.Common;

public record CourierTrendPointDto(
    DateOnly PeriodStart,
    DateOnly PeriodEnd,
    decimal SlaScore,
    decimal SuccessRate,
    decimal OnTimeDelivery,
    decimal ReturnRate,
    decimal? AvgDeliveryHours,
    int TotalAssigned,
    int TotalDelivered,
    int TotalReturned);

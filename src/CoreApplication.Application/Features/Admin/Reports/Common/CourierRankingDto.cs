namespace CoreApplication.Application.Features.Admin.Reports.Common;

public record CourierRankingDto(
    int CourierId,
    string CourierTitle,
    decimal SlaScore,
    decimal SuccessRate,
    decimal OnTimeDelivery,
    decimal ReturnRate,
    decimal? AvgDeliveryHours,
    int TotalAssigned,
    int TotalDelivered,
    DateOnly PeriodStart,
    DateOnly PeriodEnd,
    int ActiveAlerts,
    bool HasMissingKpi);

using CoreApplication.Domain.Enums;

namespace CoreApplication.Application.Features.CourierPanel.Common;

public record CourierSlaTrendPointDto(
    DateOnly PeriodStart,
    DateOnly PeriodEnd,
    SlaPeriodType PeriodType,
    decimal SlaScore,
    decimal SuccessRate,
    decimal OnTimeDelivery,
    decimal ReturnRate,
    decimal? AvgDeliveryHours,
    int TotalAssigned,
    int TotalDelivered,
    int TotalReturned);

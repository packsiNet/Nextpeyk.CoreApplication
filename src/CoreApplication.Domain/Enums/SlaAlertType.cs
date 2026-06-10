namespace CoreApplication.Domain.Enums;

public enum SlaAlertType
{
    SlaScoreLow = 0,
    SuccessRateLow = 1,
    OnTimeDeliveryLow = 2,
    ReturnRateHigh = 3,
    DeliveryTimeSlow = 4,
    CostHigh = 5,
    MissingKpiData = 6
}

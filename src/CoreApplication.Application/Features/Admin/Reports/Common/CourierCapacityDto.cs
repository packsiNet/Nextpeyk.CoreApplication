namespace CoreApplication.Application.Features.Admin.Reports.Common;

public record CourierCapacityDto(
    int CourierId,
    string CourierTitle,
    int MaxCapacityPerDay,
    decimal AvgLoad,
    int PeakLoad,
    decimal LoadPercent);

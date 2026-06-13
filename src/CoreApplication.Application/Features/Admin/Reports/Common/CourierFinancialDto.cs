namespace CoreApplication.Application.Features.Admin.Reports.Common;

public record CourierFinancialDto(
    int? CourierId,
    string? CourierTitle,
    int TotalParcels,
    decimal TotalRevenue,
    decimal AvgCostPerParcel,
    decimal PickupTotal,
    decimal DistributionTotal);

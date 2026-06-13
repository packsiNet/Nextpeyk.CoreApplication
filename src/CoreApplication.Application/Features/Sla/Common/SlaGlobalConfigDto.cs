namespace CoreApplication.Application.Features.Sla.Common;

public record SlaGlobalConfigDto(
    int Id,
    decimal BenchmarkCostPerParcel,
    decimal WeightSuccessRate,
    decimal WeightOnTimeDelivery,
    decimal WeightReturnRate,
    decimal WeightDeliveryTime,
    decimal WeightCost,
    DateTime EffectiveFrom);

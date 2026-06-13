using MediatR;

namespace CoreApplication.Application.Features.Sla.Commands.UpdateSlaGlobalConfig;

public record UpdateSlaGlobalConfigCommand(
    decimal BenchmarkCostPerParcel,
    decimal WeightSuccessRate,
    decimal WeightOnTimeDelivery,
    decimal WeightReturnRate,
    decimal WeightDeliveryTime,
    decimal WeightCost) : IRequest<int>;

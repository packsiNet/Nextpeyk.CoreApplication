using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Application.Features.Sla.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.Sla.Queries.GetSlaGlobalConfig;

public class GetSlaGlobalConfigQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetSlaGlobalConfigQuery, SlaGlobalConfigDto?>
{
    public async Task<SlaGlobalConfigDto?> Handle(GetSlaGlobalConfigQuery request, CancellationToken cancellationToken)
    {
        var config = await context.SlaGlobalConfigs
            .OrderByDescending(c => c.EffectiveFrom)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        return config is null
            ? null
            : new SlaGlobalConfigDto(
                config.Id,
                config.BenchmarkCostPerParcel,
                config.WeightSuccessRate,
                config.WeightOnTimeDelivery,
                config.WeightReturnRate,
                config.WeightDeliveryTime,
                config.WeightCost,
                config.EffectiveFrom);
    }
}

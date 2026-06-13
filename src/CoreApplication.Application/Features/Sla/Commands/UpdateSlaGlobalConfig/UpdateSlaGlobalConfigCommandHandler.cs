using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Domain.Entities.Sla;
using MediatR;

namespace CoreApplication.Application.Features.Sla.Commands.UpdateSlaGlobalConfig;

public class UpdateSlaGlobalConfigCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser,
    IDateTimeService dateTime)
    : IRequestHandler<UpdateSlaGlobalConfigCommand, int>
{
    public async Task<int> Handle(UpdateSlaGlobalConfigCommand request, CancellationToken cancellationToken)
    {
        // هر تغییر یک رکورد جدید می‌سازد — رکورد قدیمی دست نخورده باقی می‌ماند
        // چون snapshot های قبلی به config قبلی وابسته‌اند
        var config = new SlaGlobalConfig
        {
            BenchmarkCostPerParcel = request.BenchmarkCostPerParcel,
            WeightSuccessRate = request.WeightSuccessRate,
            WeightOnTimeDelivery = request.WeightOnTimeDelivery,
            WeightReturnRate = request.WeightReturnRate,
            WeightDeliveryTime = request.WeightDeliveryTime,
            WeightCost = request.WeightCost,
            EffectiveFrom = dateTime.UtcNow,
            CreatedByUserId = currentUser.UserId ?? 0,
            CreatedAt = dateTime.UtcNow
        };

        context.SlaGlobalConfigs.Add(config);
        await context.SaveChangesAsync(cancellationToken);

        return config.Id;
    }
}

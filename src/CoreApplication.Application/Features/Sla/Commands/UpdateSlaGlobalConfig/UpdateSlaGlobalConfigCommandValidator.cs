using FluentValidation;

namespace CoreApplication.Application.Features.Sla.Commands.UpdateSlaGlobalConfig;

public class UpdateSlaGlobalConfigCommandValidator : AbstractValidator<UpdateSlaGlobalConfigCommand>
{
    public UpdateSlaGlobalConfigCommandValidator()
    {
        RuleFor(x => x.BenchmarkCostPerParcel).GreaterThan(0);
        RuleFor(x => x.WeightSuccessRate).InclusiveBetween(0, 1);
        RuleFor(x => x.WeightOnTimeDelivery).InclusiveBetween(0, 1);
        RuleFor(x => x.WeightReturnRate).InclusiveBetween(0, 1);
        RuleFor(x => x.WeightDeliveryTime).InclusiveBetween(0, 1);
        RuleFor(x => x.WeightCost).InclusiveBetween(0, 1);
        RuleFor(x => x).Must(x =>
            x.WeightSuccessRate + x.WeightOnTimeDelivery +
            x.WeightReturnRate + x.WeightDeliveryTime + x.WeightCost == 1)
            .WithMessage("Sum of all weights must equal 1.");
    }
}

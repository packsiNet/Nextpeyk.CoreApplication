using FluentValidation;

namespace CoreApplication.Application.Features.Couriers.Commands.SetCourierSlaConfigs;

public class SetCourierSlaConfigsCommandValidator : AbstractValidator<SetCourierSlaConfigsCommand>
{
    public SetCourierSlaConfigsCommandValidator()
    {
        RuleFor(x => x.SlaConfigs).NotNull();
        RuleForEach(x => x.SlaConfigs).ChildRules(item =>
        {
            item.RuleFor(x => x.SlaHours).GreaterThan(0);
            item.RuleFor(x => x.SuccessRateMin).InclusiveBetween(0, 1);
            item.RuleFor(x => x.OnTimeMin).InclusiveBetween(0, 1);
            item.RuleFor(x => x.ReturnRateMax).InclusiveBetween(0, 1);
        });
    }
}

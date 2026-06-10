using FluentValidation;

namespace CoreApplication.Application.Features.Couriers.Commands.SetCourierActivityTimes;

public class SetCourierActivityTimesCommandValidator : AbstractValidator<SetCourierActivityTimesCommand>
{
    public SetCourierActivityTimesCommandValidator()
    {
        RuleFor(x => x.ActivityTimes).NotNull();
        RuleForEach(x => x.ActivityTimes).ChildRules(item =>
        {
            item.RuleFor(x => x.DayOfWeek).InclusiveBetween(0, 6);
            item.RuleFor(x => x.EndTime).GreaterThan(x => x.StartTime)
                .WithMessage("EndTime must be after StartTime.");
        });
    }
}

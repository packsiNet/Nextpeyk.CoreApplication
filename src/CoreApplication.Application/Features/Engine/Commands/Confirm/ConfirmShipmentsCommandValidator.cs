using FluentValidation;

namespace CoreApplication.Application.Features.Engine.Commands.Confirm;

public class ConfirmShipmentsCommandValidator : AbstractValidator<ConfirmShipmentsCommand>
{
    public ConfirmShipmentsCommandValidator()
    {
        RuleFor(x => x.Weight).GreaterThan(0);
        RuleFor(x => x.Shipments).NotEmpty();
        RuleForEach(x => x.Shipments).ChildRules(s =>
        {
            s.RuleFor(x => x.Collection).NotNull();
            s.RuleFor(x => x.Distribution).NotNull();
            s.RuleFor(x => x.CollectFirstName).NotEmpty();
            s.RuleFor(x => x.CollectLastName).NotEmpty();
            s.RuleFor(x => x.CollectPhoneNumber).NotEmpty();
            s.RuleFor(x => x.CollectAddress).NotEmpty();
            s.RuleFor(x => x.DistributeFirstName).NotEmpty();
            s.RuleFor(x => x.DistributeLastName).NotEmpty();
            s.RuleFor(x => x.DistributePhoneNumber).NotEmpty();
            s.RuleFor(x => x.DistributeAddress).NotEmpty();
        });
    }
}

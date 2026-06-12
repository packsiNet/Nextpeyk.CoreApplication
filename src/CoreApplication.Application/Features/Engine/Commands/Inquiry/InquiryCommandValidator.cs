using FluentValidation;

namespace CoreApplication.Application.Features.Engine.Commands.Inquiry;

public class InquiryCommandValidator : AbstractValidator<InquiryCommand>
{
    public InquiryCommandValidator()
    {
        RuleFor(x => x.Weight).GreaterThan(0);
        RuleFor(x => x.Shipments).NotEmpty();
        RuleForEach(x => x.Shipments).ChildRules(s =>
        {
            s.RuleFor(x => x.Collection).NotNull();
            s.RuleFor(x => x.Distribution).NotNull();
        });
    }
}

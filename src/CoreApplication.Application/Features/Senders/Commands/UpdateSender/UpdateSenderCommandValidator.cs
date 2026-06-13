using FluentValidation;

namespace CoreApplication.Application.Features.Senders.Commands.UpdateSender;

public class UpdateSenderCommandValidator : AbstractValidator<UpdateSenderCommand>
{
    public UpdateSenderCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.NationalCode).MaximumLength(20);
        RuleFor(x => x.EconomicCode).MaximumLength(20);
        RuleFor(x => x.RegistrationNumber).MaximumLength(50);
        RuleFor(x => x.ContractNumber).MaximumLength(50);
        RuleFor(x => x.PhoneNumber).MaximumLength(20);
        RuleFor(x => x.Email).MaximumLength(200).EmailAddress().When(x => x.Email != null);
        RuleFor(x => x.ContractEndDate)
            .GreaterThan(x => x.ContractStartDate)
            .When(x => x.ContractStartDate.HasValue && x.ContractEndDate.HasValue)
            .WithMessage("Contract end date must be after start date.");
    }
}

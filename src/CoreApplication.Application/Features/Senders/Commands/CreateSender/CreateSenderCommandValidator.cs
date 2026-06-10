using FluentValidation;
using CoreApplication.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.Senders.Commands.CreateSender;

public class CreateSenderCommandValidator : AbstractValidator<CreateSenderCommand>
{
    public CreateSenderCommandValidator(IApplicationDbContext context)
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Email).EmailAddress().When(x => x.Email is not null);
        RuleFor(x => x.ContractEndDate)
            .GreaterThan(x => x.ContractStartDate)
            .When(x => x.ContractStartDate.HasValue && x.ContractEndDate.HasValue)
            .WithMessage("Contract end date must be after start date.");
        RuleFor(x => x.UserName)
            .NotEmpty().MaximumLength(100)
            .MustAsync(async (username, ct) =>
                !await context.UserAccounts.AnyAsync(u => u.UserName == username, ct))
            .WithMessage("Username already exists.");
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
    }
}

using CoreApplication.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.Couriers.Commands.CreateBoxSize;

public class CreateBoxSizeCommandValidator : AbstractValidator<CreateBoxSizeCommand>
{
    public CreateBoxSizeCommandValidator(IApplicationDbContext context)
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(50)
            .MustAsync(async (code, ct) =>
                !await context.BoxSizes.AnyAsync(b => b.Code == code && !b.IsDeleted, ct))
            .WithMessage("Box size code already exists.");
        RuleFor(x => x.BoxLength).GreaterThan(0);
        RuleFor(x => x.BoxWidth).GreaterThan(0);
        RuleFor(x => x.BoxHeight).GreaterThan(0);
    }
}

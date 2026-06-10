using CoreApplication.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.Geography.Commands.UpdateCity;

public class UpdateCityCommandValidator : AbstractValidator<UpdateCityCommand>
{
    public UpdateCityCommandValidator(IApplicationDbContext context)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100)
            .MustAsync(async (cmd, name, ct) =>
                !await context.Cities.AnyAsync(c => c.Name == name && c.Id != cmd.Id && !c.IsDeleted, ct))
            .WithMessage("City name already exists.");
    }
}

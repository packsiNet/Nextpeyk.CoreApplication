using FluentValidation;

namespace CoreApplication.Application.Features.Fleets.Commands.CreateFleetType;

public class CreateFleetTypeCommandValidator : AbstractValidator<CreateFleetTypeCommand>
{
    public CreateFleetTypeCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
    }
}

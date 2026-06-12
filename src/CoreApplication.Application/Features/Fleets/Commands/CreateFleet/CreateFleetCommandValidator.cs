using FluentValidation;

namespace CoreApplication.Application.Features.Fleets.Commands.CreateFleet;

public class CreateFleetCommandValidator : AbstractValidator<CreateFleetCommand>
{
    public CreateFleetCommandValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
        RuleFor(x => x.PhoneNumber).NotEmpty().MaximumLength(20);
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.FleetTypeId).GreaterThan(0);
        RuleFor(x => x.Plaque).NotEmpty().MaximumLength(20);
        RuleFor(x => x.DrivingLicense).NotEmpty().MaximumLength(50);
        RuleFor(x => x.StartDate).NotEmpty();
    }
}

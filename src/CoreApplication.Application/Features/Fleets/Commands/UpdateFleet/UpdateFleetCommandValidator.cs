using FluentValidation;

namespace CoreApplication.Application.Features.Fleets.Commands.UpdateFleet;

public class UpdateFleetCommandValidator : AbstractValidator<UpdateFleetCommand>
{
    public UpdateFleetCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.FleetTypeId).GreaterThan(0);
        RuleFor(x => x.Plaque).NotEmpty().MaximumLength(20);
        RuleFor(x => x.DrivingLicense).NotEmpty().MaximumLength(50);
        RuleFor(x => x.StartDate).NotEmpty();
    }
}

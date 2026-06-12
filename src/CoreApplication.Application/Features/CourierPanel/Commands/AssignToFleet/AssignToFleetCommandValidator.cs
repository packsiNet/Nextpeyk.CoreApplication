using FluentValidation;

namespace CoreApplication.Application.Features.CourierPanel.Commands.AssignToFleet;

public class AssignToFleetCommandValidator : AbstractValidator<AssignToFleetCommand>
{
    public AssignToFleetCommandValidator()
    {
        RuleFor(x => x.FleetId).GreaterThan(0);
        RuleFor(x => x.ParcelCourierIds).NotEmpty();
    }
}

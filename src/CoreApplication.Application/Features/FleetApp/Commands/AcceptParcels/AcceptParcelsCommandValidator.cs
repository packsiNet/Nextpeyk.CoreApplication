using FluentValidation;

namespace CoreApplication.Application.Features.FleetApp.Commands.AcceptParcels;

public class AcceptParcelsCommandValidator : AbstractValidator<AcceptParcelsCommand>
{
    public AcceptParcelsCommandValidator()
    {
        RuleFor(x => x.ParcelCourierFleetIds).NotEmpty();
    }
}

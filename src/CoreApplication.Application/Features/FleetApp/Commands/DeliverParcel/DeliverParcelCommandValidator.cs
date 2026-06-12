using FluentValidation;

namespace CoreApplication.Application.Features.FleetApp.Commands.DeliverParcel;

public class DeliverParcelCommandValidator : AbstractValidator<DeliverParcelCommand>
{
    public DeliverParcelCommandValidator()
    {
        RuleFor(x => x.ParcelCourierFleetId).GreaterThan(0);
        RuleFor(x => x.ReceiverName).NotEmpty().MaximumLength(200);
    }
}

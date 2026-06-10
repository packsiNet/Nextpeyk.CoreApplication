using FluentValidation;

namespace CoreApplication.Application.Features.Parcels.Commands.RegisterParcel;

public class RegisterParcelCommandValidator : AbstractValidator<RegisterParcelCommand>
{
    public RegisterParcelCommandValidator()
    {
        RuleFor(x => x.SenderFirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.SenderLastName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.SenderPhoneNumber).NotEmpty().MaximumLength(20);
        RuleFor(x => x.SenderAddress).NotEmpty().MaximumLength(500);
        RuleFor(x => x.ReceiverFirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.ReceiverLastName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.ReceiverPhoneNumber).NotEmpty().MaximumLength(20);
        RuleFor(x => x.ReceiverAddress).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Weight).GreaterThan(0);
        RuleFor(x => x.Length).GreaterThan(0);
        RuleFor(x => x.Width).GreaterThan(0);
        RuleFor(x => x.Height).GreaterThan(0);
        RuleFor(x => x.Value).GreaterThanOrEqualTo(0);
        RuleFor(x => x.ReceiverLatitude).InclusiveBetween(-90, 90);
        RuleFor(x => x.ReceiverLongitude).InclusiveBetween(-180, 180);
        RuleFor(x => x.SenderLatitude).InclusiveBetween(-90, 90);
        RuleFor(x => x.SenderLongitude).InclusiveBetween(-180, 180);
    }
}

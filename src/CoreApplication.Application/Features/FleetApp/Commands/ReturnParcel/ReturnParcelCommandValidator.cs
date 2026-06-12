using CoreApplication.Domain.Enums;
using FluentValidation;

namespace CoreApplication.Application.Features.FleetApp.Commands.ReturnParcel;

public class ReturnParcelCommandValidator : AbstractValidator<ReturnParcelCommand>
{
    public ReturnParcelCommandValidator()
    {
        RuleFor(x => x.ParcelCourierFleetId).GreaterThan(0);
        RuleFor(x => x.ReturnReason).IsInEnum();
        RuleFor(x => x.ReturnNote)
            .NotEmpty()
            .When(x => x.ReturnReason == ReturnParcelReason.Other)
            .WithMessage("ReturnNote required when reason is Other.");
        RuleFor(x => x.ImagePaths).NotEmpty().WithMessage("At least one image required for return.");
    }
}

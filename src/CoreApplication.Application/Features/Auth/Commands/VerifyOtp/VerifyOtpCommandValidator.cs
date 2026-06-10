using FluentValidation;

namespace CoreApplication.Application.Features.Auth.Commands.VerifyOtp;

public class VerifyOtpCommandValidator : AbstractValidator<VerifyOtpCommand>
{
    public VerifyOtpCommandValidator()
    {
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .Matches(@"^09[0-9]{9}$").WithMessage("Invalid Iranian phone number format.");

        RuleFor(x => x.Code)
            .NotEmpty()
            .Length(6).WithMessage("OTP code must be 6 digits.")
            .Matches(@"^\d{6}$").WithMessage("OTP code must contain only digits.");
    }
}

using FluentValidation;

namespace CoreApplication.Application.Features.Auth.Commands.SendOtp;

public class SendOtpCommandValidator : AbstractValidator<SendOtpCommand>
{
    public SendOtpCommandValidator()
    {
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .Matches(@"^09[0-9]{9}$").WithMessage("Invalid Iranian phone number format.");
    }
}

using CoreApplication.Application.Features.Auth.Common;
using MediatR;

namespace CoreApplication.Application.Features.Auth.Commands.VerifyOtp;

public record VerifyOtpCommand(string PhoneNumber, string Code) : IRequest<AuthResult>;

using MediatR;

namespace CoreApplication.Application.Features.Auth.Commands.SendOtp;

public record SendOtpCommand(string PhoneNumber) : IRequest;

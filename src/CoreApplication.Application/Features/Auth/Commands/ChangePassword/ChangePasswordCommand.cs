using MediatR;

namespace CoreApplication.Application.Features.Auth.Commands.ChangePassword;

public record ChangePasswordCommand(
    string CurrentPassword,
    string NewPassword) : IRequest;

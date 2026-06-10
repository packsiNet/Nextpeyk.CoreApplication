using CoreApplication.Application.Features.Auth.Common;
using MediatR;

namespace CoreApplication.Application.Features.Auth.Commands.Login;

public record LoginCommand(string Identifier, string Password) : IRequest<AuthResult>;

using CoreApplication.Application.Common.Exceptions;
using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Application.Features.Auth.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler(
    IApplicationDbContext context,
    IPasswordService passwordService,
    IJwtService jwtService)
    : IRequestHandler<LoginCommand, AuthResult>
{
    public async Task<AuthResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var identifier = request.Identifier.Trim();

        var user = await context.UserAccounts
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .Where(u => u.IsActive && !u.IsDeleted)
            .Where(u => u.UserName == identifier || u.PhoneNumber == identifier)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException("UserAccount", identifier);

        if (!passwordService.Verify(request.Password, user.Password))
            throw new UnauthorizedAccessException("Invalid credentials.");

        var roles = user.UserRoles
            .Where(ur => ur.IsActive && !ur.IsDeleted)
            .Select(ur => ur.Role.RoleName)
            .ToList();

        var token = jwtService.GenerateToken(user, roles);

        return new AuthResult(token, user.Id, user.UserName, roles, user.CourierId, user.MustChangePassword);
    }
}

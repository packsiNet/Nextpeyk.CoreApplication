using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Application.Features.Auth.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.Auth.Commands.VerifyOtp;

public class VerifyOtpCommandHandler(
    IApplicationDbContext context,
    IJwtService jwtService,
    IDateTimeService dateTime)
    : IRequestHandler<VerifyOtpCommand, AuthResult>
{
    public async Task<AuthResult> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
    {
        var otp = await context.OtpCodes
            .Where(o => o.PhoneNumber == request.PhoneNumber
                     && o.Code == request.Code
                     && !o.IsUsed
                     && o.ExpiresAt > dateTime.UtcNow)
            .OrderByDescending(o => o.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);

        if (otp is null)
            throw new UnauthorizedAccessException("Invalid or expired OTP code.");

        otp.IsUsed = true;

        var user = await context.UserAccounts
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .Where(u => u.PhoneNumber == request.PhoneNumber && u.IsActive && !u.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new UnauthorizedAccessException("User not found.");

        await context.SaveChangesAsync(cancellationToken);

        var roles = user.UserRoles
            .Where(ur => ur.IsActive && !ur.IsDeleted)
            .Select(ur => ur.Role.RoleName)
            .ToList();

        var token = jwtService.GenerateToken(user, roles);

        return new AuthResult(token, user.Id, user.UserName, roles, user.CourierId);
    }
}

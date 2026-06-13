using CoreApplication.Application.Common.Exceptions;
using CoreApplication.Application.Common.Interfaces;
using MediatR;

namespace CoreApplication.Application.Features.Auth.Commands.ChangePassword;

public class ChangePasswordCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser,
    IPasswordService passwordService,
    IDateTimeService dateTime)
    : IRequestHandler<ChangePasswordCommand>
{
    public async Task Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId
            ?? throw new ForbiddenAccessException();

        var user = await context.UserAccounts.FindAsync([userId], cancellationToken)
            ?? throw new NotFoundException("UserAccount", userId);

        if (!passwordService.Verify(request.CurrentPassword, user.Password))
            throw new UnauthorizedAccessException("Current password is incorrect.");

        user.Password = passwordService.Hash(request.NewPassword);
        user.MustChangePassword = false;
        user.SecurityStamp = Guid.NewGuid().ToString();
        user.ModifiedByUserId = userId;
        user.ModifiedDateTime = dateTime.UtcNow;

        await context.SaveChangesAsync(cancellationToken);
    }
}

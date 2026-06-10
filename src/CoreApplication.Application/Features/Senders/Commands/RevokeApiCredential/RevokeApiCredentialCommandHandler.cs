using CoreApplication.Application.Common.Exceptions;
using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Domain.Entities.Sender;
using MediatR;

namespace CoreApplication.Application.Features.Senders.Commands.RevokeApiCredential;

public class RevokeApiCredentialCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser,
    IDateTimeService dateTime)
    : IRequestHandler<RevokeApiCredentialCommand>
{
    public async Task Handle(RevokeApiCredentialCommand request, CancellationToken cancellationToken)
    {
        var credential = await context.SenderApiCredentials.FindAsync(
            [request.CredentialId], cancellationToken)
            ?? throw new NotFoundException(nameof(SenderApiCredential), request.CredentialId);

        credential.IsActive = false;
        credential.ModifiedByUserId = currentUser.UserId ?? 0;
        credential.ModifiedDateTime = dateTime.UtcNow;

        await context.SaveChangesAsync(cancellationToken);
    }
}

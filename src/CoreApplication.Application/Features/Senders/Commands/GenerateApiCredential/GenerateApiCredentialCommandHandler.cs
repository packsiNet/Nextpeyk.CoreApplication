using System.Security.Cryptography;
using CoreApplication.Application.Common.Exceptions;
using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Domain.Entities.Sender;
using MediatR;

namespace CoreApplication.Application.Features.Senders.Commands.GenerateApiCredential;

public class GenerateApiCredentialCommandHandler(
    IApplicationDbContext context,
    IPasswordService passwordService,
    ICurrentUserService currentUser,
    IDateTimeService dateTime)
    : IRequestHandler<GenerateApiCredentialCommand, GenerateApiCredentialResult>
{
    public async Task<GenerateApiCredentialResult> Handle(
        GenerateApiCredentialCommand request,
        CancellationToken cancellationToken)
    {
        var sender = await context.Senders.FindAsync([request.SenderId], cancellationToken)
            ?? throw new NotFoundException(nameof(Sender), request.SenderId);

        if (!sender.IsActive || sender.IsDeleted)
            throw new InvalidOperationException("Cannot generate credentials for an inactive sender.");

        var apiKey = GenerateSecureToken(32);
        var apiSecret = GenerateSecureToken(48);

        var credential = new SenderApiCredential
        {
            SenderId = request.SenderId,
            ApiKey = apiKey,
            ApiSecretHash = passwordService.Hash(apiSecret),
            Description = request.Description,
            ExpiresAt = request.ExpiresAt,
            CreatedByUserId = currentUser.UserId ?? 0,
            ModifiedByUserId = currentUser.UserId ?? 0,
            ModifiedDateTime = dateTime.UtcNow,
            CreatedAt = dateTime.UtcNow,
            IsActive = true
        };

        context.SenderApiCredentials.Add(credential);
        await context.SaveChangesAsync(cancellationToken);

        // ApiSecret returned ONCE — never retrievable again
        return new GenerateApiCredentialResult(credential.Id, apiKey, apiSecret);
    }

    private static string GenerateSecureToken(int byteLength)
        => Convert.ToBase64String(RandomNumberGenerator.GetBytes(byteLength))
            .Replace("+", "-").Replace("/", "_").Replace("=", "");
}

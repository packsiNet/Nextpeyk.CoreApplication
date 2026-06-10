using MediatR;

namespace CoreApplication.Application.Features.Senders.Commands.GenerateApiCredential;

public record GenerateApiCredentialCommand(
    int SenderId,
    string? Description,
    DateTime? ExpiresAt) : IRequest<GenerateApiCredentialResult>;

public record GenerateApiCredentialResult(
    int CredentialId,
    string ApiKey,
    string ApiSecret);

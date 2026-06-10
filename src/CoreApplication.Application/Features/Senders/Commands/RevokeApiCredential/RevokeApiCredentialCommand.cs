using MediatR;

namespace CoreApplication.Application.Features.Senders.Commands.RevokeApiCredential;

public record RevokeApiCredentialCommand(int CredentialId) : IRequest;

using MediatR;

namespace CoreApplication.Application.Features.Senders.Commands.DeleteSender;

public record DeleteSenderCommand(int Id) : IRequest;

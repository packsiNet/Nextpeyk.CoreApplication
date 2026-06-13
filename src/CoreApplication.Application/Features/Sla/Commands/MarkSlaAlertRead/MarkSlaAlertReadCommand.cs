using MediatR;

namespace CoreApplication.Application.Features.Sla.Commands.MarkSlaAlertRead;

public record MarkSlaAlertReadCommand(int AlertId) : IRequest;

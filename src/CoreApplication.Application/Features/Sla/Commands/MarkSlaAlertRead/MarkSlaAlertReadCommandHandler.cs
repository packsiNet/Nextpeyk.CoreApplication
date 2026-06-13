using CoreApplication.Application.Common.Exceptions;
using CoreApplication.Application.Common.Interfaces;
using MediatR;

namespace CoreApplication.Application.Features.Sla.Commands.MarkSlaAlertRead;

public class MarkSlaAlertReadCommandHandler(IApplicationDbContext context)
    : IRequestHandler<MarkSlaAlertReadCommand>
{
    public async Task Handle(MarkSlaAlertReadCommand request, CancellationToken cancellationToken)
    {
        var alert = await context.SlaAlerts.FindAsync([request.AlertId], cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Sla.SlaAlert), request.AlertId);

        if (alert.IsRead) return;

        alert.IsRead = true;
        await context.SaveChangesAsync(cancellationToken);
    }
}

using CoreApplication.Application.Common.Exceptions;
using CoreApplication.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.Senders.Commands.DeleteSender;

public class DeleteSenderCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser,
    IDateTimeService dateTime)
    : IRequestHandler<DeleteSenderCommand>
{
    public async Task Handle(DeleteSenderCommand request, CancellationToken cancellationToken)
    {
        var sender = await context.Senders
            .Where(s => s.Id == request.Id && !s.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Sender.Sender), request.Id);

        var now = dateTime.UtcNow;
        var userId = currentUser.UserId ?? 0;

        sender.IsDeleted = true;
        sender.IsActive = false;
        sender.ModifiedByUserId = userId;
        sender.ModifiedDateTime = now;

        // غیرفعال کردن همه UserAccountهای مرتبط
        var userAccounts = await context.UserAccounts
            .Where(u => u.SenderId == request.Id && u.IsActive)
            .ToListAsync(cancellationToken);

        foreach (var user in userAccounts)
        {
            user.IsActive = false;
            user.ModifiedByUserId = userId;
            user.ModifiedDateTime = now;
        }

        await context.SaveChangesAsync(cancellationToken);
    }
}

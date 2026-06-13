using CoreApplication.Application.Common.Exceptions;
using CoreApplication.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.FleetApp.Commands.EndWorkSession;

public class EndWorkSessionCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser,
    IDateTimeService dateTime)
    : IRequestHandler<EndWorkSessionCommand>
{
    public async Task Handle(EndWorkSessionCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId ?? throw new ForbiddenAccessException();
        var now = dateTime.UtcNow;
        var today = DateOnly.FromDateTime(now);

        var fleet = await context.Fleets
            .FirstOrDefaultAsync(f => f.UserAccountId == userId && f.IsActive && !f.IsDeleted, cancellationToken)
            ?? throw new NotFoundException("Fleet", userId);

        var session = await context.CourierWorkSessions
            .FirstOrDefaultAsync(s =>
                s.FleetId == fleet.Id &&
                s.SessionDate == today &&
                s.EndedAt == null &&
                s.IsActive && !s.IsDeleted, cancellationToken)
            ?? throw new NotFoundException("WorkSession", fleet.Id);

        session.EndedAt = now;
        session.ModifiedByUserId = userId;
        session.ModifiedDateTime = now;

        await context.SaveChangesAsync(cancellationToken);
    }
}

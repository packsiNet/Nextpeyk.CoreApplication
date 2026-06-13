using CoreApplication.Application.Common.Exceptions;
using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Domain.Entities.Tracking;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace CoreApplication.Application.Features.FleetApp.Commands.RecordLocationBatch;

public class RecordLocationBatchCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser,
    IDateTimeService dateTime,
    ILiveLocationBroadcaster broadcaster)
    : IRequestHandler<RecordLocationBatchCommand>
{
    private const double MinDistanceMeters = 20.0;
    private const int MaxGapMinutesForHeartbeat = 5;
    private static readonly GeometryFactory GeomFactory = new GeometryFactory(new PrecisionModel(), 4326);

    public async Task Handle(RecordLocationBatchCommand request, CancellationToken cancellationToken)
    {
        if (request.Points.Count == 0) return;

        var userId = currentUser.UserId ?? throw new ForbiddenAccessException();
        var now = dateTime.UtcNow;
        var today = DateOnly.FromDateTime(now);

        var fleet = await context.Fleets
            .FirstOrDefaultAsync(f => f.UserAccountId == userId && f.IsActive && !f.IsDeleted, cancellationToken)
            ?? throw new NotFoundException("Fleet", userId);

        var session = await GetOrCreateSessionAsync(fleet.Id, fleet.CourierId, today, now, userId, cancellationToken);

        var lastTrack = await context.CourierLocationTracks
            .Where(t => t.WorkSessionId == session.Id)
            .OrderByDescending(t => t.RecordedAt)
            .FirstOrDefaultAsync(cancellationToken);

        var newTracks = new List<CourierLocationTrack>();
        double addedDistance = 0;

        var sortedPoints = request.Points
            .Where(p => p.RecordedAt <= now && p.RecordedAt >= session.StartedAt)
            .OrderBy(p => p.RecordedAt)
            .ToList();

        double prevLat = lastTrack?.Latitude ?? 0;
        double prevLon = lastTrack?.Longitude ?? 0;
        DateTime? prevTime = lastTrack?.RecordedAt;

        foreach (var point in sortedPoints)
        {
            bool isFirstPoint = lastTrack == null && newTracks.Count == 0;
            bool distanceOk = isFirstPoint || HaversineMeters(prevLat, prevLon, point.Latitude, point.Longitude) >= MinDistanceMeters;
            bool heartbeatNeeded = prevTime.HasValue && (point.RecordedAt - prevTime.Value).TotalMinutes >= MaxGapMinutesForHeartbeat;

            if (!distanceOk && !heartbeatNeeded) continue;

            if (!isFirstPoint && prevLat != 0)
                addedDistance += HaversineMeters(prevLat, prevLon, point.Latitude, point.Longitude);

            var track = new CourierLocationTrack
            {
                WorkSessionId = session.Id,
                FleetId = fleet.Id,
                CourierId = fleet.CourierId,
                Latitude = point.Latitude,
                Longitude = point.Longitude,
                Location = GeomFactory.CreatePoint(new Coordinate(point.Longitude, point.Latitude)),
                Accuracy = point.Accuracy,
                Speed = point.Speed,
                Heading = point.Heading,
                RecordedAt = point.RecordedAt,
                CreatedAt = now
            };

            newTracks.Add(track);
            prevLat = point.Latitude;
            prevLon = point.Longitude;
            prevTime = point.RecordedAt;
        }

        if (newTracks.Count == 0) return;

        context.CourierLocationTracks.AddRange(newTracks);

        session.TotalDistanceMeters += addedDistance;
        session.ModifiedByUserId = userId;
        session.ModifiedDateTime = now;

        await context.SaveChangesAsync(cancellationToken);

        var latest = newTracks[^1];
        var driverName = await context.UserAccounts
            .Where(u => u.Id == userId)
            .Select(u => (u.FirstName + " " + u.LastName).Trim())
            .FirstOrDefaultAsync(cancellationToken);

        await broadcaster.BroadcastAsync(new LiveLocationDto(
            fleet.Id,
            fleet.CourierId,
            driverName,
            latest.Latitude,
            latest.Longitude,
            latest.Speed,
            latest.Heading,
            latest.RecordedAt), cancellationToken);
    }

    private async Task<CourierWorkSession> GetOrCreateSessionAsync(
        int fleetId, int courierId, DateOnly today, DateTime now, int userId, CancellationToken ct)
    {
        var session = await context.CourierWorkSessions
            .FirstOrDefaultAsync(s => s.FleetId == fleetId && s.SessionDate == today && s.IsActive && !s.IsDeleted, ct);

        if (session is not null) return session;

        session = new CourierWorkSession
        {
            FleetId = fleetId,
            CourierId = courierId,
            SessionDate = today,
            StartedAt = now,
            CreatedAt = now,
            CreatedByUserId = userId,
            ModifiedByUserId = userId,
            ModifiedDateTime = now,
            IsActive = true
        };

        context.CourierWorkSessions.Add(session);
        await context.SaveChangesAsync(ct);

        return session;
    }

    private static double HaversineMeters(double lat1, double lon1, double lat2, double lon2)
    {
        const double R = 6371000;
        var dLat = (lat2 - lat1) * Math.PI / 180;
        var dLon = (lon2 - lon1) * Math.PI / 180;
        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2)
              + Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180)
              * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        return R * 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
    }
}

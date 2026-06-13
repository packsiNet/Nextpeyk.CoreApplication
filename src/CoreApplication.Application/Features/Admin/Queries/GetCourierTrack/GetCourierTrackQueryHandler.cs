using CoreApplication.Application.Common.Exceptions;
using CoreApplication.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.Admin.Queries.GetCourierTrack;

public class GetCourierTrackQueryHandler(
    IApplicationDbContext context,
    IRouteSimplificationService simplifier,
    IMapMatchingService mapMatcher)
    : IRequestHandler<GetCourierTrackQuery, CourierTrackDto>
{
    public async Task<CourierTrackDto> Handle(GetCourierTrackQuery request, CancellationToken cancellationToken)
    {
        // If caller is courier-scoped, verify the fleet belongs to their courier
        if (request.RequireCourierId.HasValue)
        {
            var fleetCourierId = await context.Fleets
                .Where(f => f.Id == request.FleetId && f.IsActive && !f.IsDeleted)
                .Select(f => (int?)f.CourierId)
                .FirstOrDefaultAsync(cancellationToken);

            if (fleetCourierId != request.RequireCourierId.Value)
                throw new ForbiddenAccessException();
        }

        var session = await context.CourierWorkSessions
            .FirstOrDefaultAsync(s =>
                s.FleetId == request.FleetId &&
                s.SessionDate == request.Date &&
                s.IsActive && !s.IsDeleted, cancellationToken)
            ?? throw new NotFoundException("WorkSession", request.FleetId);

        var tracks = await context.CourierLocationTracks
            .Where(t => t.WorkSessionId == session.Id)
            .OrderBy(t => t.RecordedAt)
            .Select(t => new TrackPointDto
            {
                Latitude = t.Latitude,
                Longitude = t.Longitude,
                Speed = t.Speed,
                Heading = t.Heading,
                Accuracy = t.Accuracy,
                RecordedAt = t.RecordedAt
            })
            .ToListAsync(cancellationToken);

        var result = new CourierTrackDto
        {
            FleetId = session.FleetId,
            CourierId = session.CourierId,
            Date = session.SessionDate,
            StartedAt = session.StartedAt,
            EndedAt = session.EndedAt,
            TotalDistanceMeters = session.TotalDistanceMeters,
            Points = tracks
        };

        var rawPoints = tracks.Select(t => (t.Latitude, t.Longitude)).ToList();

        if (request.Simplify && rawPoints.Count > 2)
        {
            var simplified = simplifier.Simplify(rawPoints, toleranceMeters: 15.0);
            result.SimplifiedPoints = simplified
                .Select(p => new TrackPointDto { Latitude = p.Lat, Longitude = p.Lon })
                .ToList();
        }

        if (request.WithMapMatching && tracks.Count >= 2)
        {
            var pointsWithTime = tracks
                .Select(t => (t.Latitude, t.Longitude, t.RecordedAt))
                .ToList();

            var matched = await mapMatcher.MatchAsync(pointsWithTime, cancellationToken);
            if (matched is not null)
                result.MatchedRoute = matched.Points;
        }

        return result;
    }
}

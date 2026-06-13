using CoreApplication.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.Admin.Queries.GetActiveDriverLocations;

public class GetActiveDriverLocationsQueryHandler(
    IApplicationDbContext context,
    IDateTimeService dateTime)
    : IRequestHandler<GetActiveDriverLocationsQuery, List<ActiveDriverLocationDto>>
{
    public async Task<List<ActiveDriverLocationDto>> Handle(
        GetActiveDriverLocationsQuery request, CancellationToken cancellationToken)
    {
        var today = DateOnly.FromDateTime(dateTime.UtcNow);

        var query = context.CourierWorkSessions
            .Where(s => s.SessionDate == today && s.EndedAt == null && s.IsActive && !s.IsDeleted);

        if (request.CourierId.HasValue)
            query = query.Where(s => s.CourierId == request.CourierId.Value);

        var sessions = await query
            .Select(s => new
            {
                s.FleetId,
                s.CourierId,
                s.StartedAt,
                s.TotalDistanceMeters,
                DriverName = s.Fleet.UserAccount.FirstName + " " + s.Fleet.UserAccount.LastName,
                LastTrack = s.LocationTracks
                    .OrderByDescending(t => t.RecordedAt)
                    .Select(t => new
                    {
                        t.Latitude,
                        t.Longitude,
                        t.Speed,
                        t.Heading,
                        t.RecordedAt
                    })
                    .FirstOrDefault()
            })
            .ToListAsync(cancellationToken);

        return sessions.Select(s => new ActiveDriverLocationDto
        {
            FleetId = s.FleetId,
            CourierId = s.CourierId,
            DriverName = s.DriverName?.Trim(),
            SessionStartedAt = s.StartedAt,
            TotalDistanceMeters = s.TotalDistanceMeters,
            LastLatitude = s.LastTrack?.Latitude,
            LastLongitude = s.LastTrack?.Longitude,
            LastSpeed = s.LastTrack?.Speed,
            LastHeading = s.LastTrack?.Heading,
            LastSeenAt = s.LastTrack?.RecordedAt
        }).ToList();
    }
}

using MediatR;

namespace CoreApplication.Application.Features.Admin.Queries.GetCourierTrack;

public record GetCourierTrackQuery(
    int FleetId,
    DateOnly Date,
    bool Simplify = false,
    bool WithMapMatching = false,
    int? RequireCourierId = null) : IRequest<CourierTrackDto>;

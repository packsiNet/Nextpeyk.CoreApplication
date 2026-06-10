using CoreApplication.Domain.Enums;
using MediatR;
using NetTopologySuite.Geometries;

namespace CoreApplication.Application.Features.Couriers.Commands.AddCourierZone;

public record AddCourierZoneCommand(
    int CourierId,
    ZoneType ZoneType,
    string Name,
    int? CityId,
    Geometry? Boundary,
    Point? CenterPoint,
    decimal Radius,
    decimal? Price) : IRequest<int>;

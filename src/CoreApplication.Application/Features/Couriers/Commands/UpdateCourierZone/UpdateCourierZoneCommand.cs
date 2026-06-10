using CoreApplication.Domain.Enums;
using MediatR;
using NetTopologySuite.Geometries;

namespace CoreApplication.Application.Features.Couriers.Commands.UpdateCourierZone;

public record UpdateCourierZoneCommand(
    int Id,
    ZoneType ZoneType,
    string Name,
    int? CityId,
    Geometry? Boundary,
    Point? CenterPoint,
    decimal Radius,
    decimal? Price) : IRequest;

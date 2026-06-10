using CoreApplication.Domain.Enums;
using NetTopologySuite.Geometries;

namespace CoreApplication.Application.Features.Couriers.Common;

public record CourierZoneDto(
    int Id,
    ZoneType ZoneType,
    string Name,
    int? CityId,
    string? CityName,
    Geometry? Boundary,
    Point? CenterPoint,
    decimal Radius,
    decimal? Price);

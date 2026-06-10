using CoreApplication.Domain.Common;
using CoreApplication.Domain.Enums;

namespace CoreApplication.Domain.Entities.Courier;

public class CourierZone : AuditableEntity
{
    public int CourierId { get; set; }
    public ZoneType ZoneType { get; set; }
    public string Name { get; set; } = default!;
    public int? CityId { get; set; }
    public string? Boundary { get; set; }
    public string? CenterPoint { get; set; }
    public decimal Radius { get; set; }
    public decimal? Price { get; set; }

    public Courier Courier { get; set; } = default!;
    public Geography.City? City { get; set; }
}

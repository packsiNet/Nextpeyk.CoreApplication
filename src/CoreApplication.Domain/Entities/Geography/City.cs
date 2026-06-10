using CoreApplication.Domain.Common;

namespace CoreApplication.Domain.Entities.Geography;

public class City : BaseEntity
{
    public string Name { get; set; } = default!;
    public string? Boundary { get; set; }

    public ICollection<Courier.CourierZone> CourierZones { get; set; } = [];
}

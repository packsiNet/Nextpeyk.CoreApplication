using CoreApplication.Domain.Common;

namespace CoreApplication.Domain.Entities.Fleet;

public class FleetType : AuditableEntity
{
    public string Title { get; set; } = default!;

    public ICollection<FleetEntity> Fleets { get; set; } = [];
}

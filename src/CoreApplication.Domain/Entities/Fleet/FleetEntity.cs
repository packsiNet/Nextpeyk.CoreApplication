using CoreApplication.Domain.Common;

namespace CoreApplication.Domain.Entities.Fleet;

public class FleetEntity : AuditableEntity
{
    public int UserAccountId { get; set; }
    public int CourierId { get; set; }
    public int FleetTypeId { get; set; }
    public string Plaque { get; set; } = default!;
    public string DrivingLicense { get; set; } = default!;
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string? Description { get; set; }
    public string? ProfileImage { get; set; }
    public string? InsuranceCode { get; set; }

    public Auth.UserAccount UserAccount { get; set; } = default!;
    public Courier.Courier Courier { get; set; } = default!;
    public FleetType FleetType { get; set; } = default!;
    public ICollection<Shipment.ParcelCourierFleet> ParcelCourierFleets { get; set; } = [];
}

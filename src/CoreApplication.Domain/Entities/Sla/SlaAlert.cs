using CoreApplication.Domain.Enums;

namespace CoreApplication.Domain.Entities.Sla;

public class SlaAlert
{
    public int Id { get; set; }
    public int CourierId { get; set; }
    public int SnapshotId { get; set; }
    public SlaAlertType AlertType { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }

    public Courier.Courier Courier { get; set; } = default!;
    public CarrierSlaSnapshot Snapshot { get; set; } = default!;
}

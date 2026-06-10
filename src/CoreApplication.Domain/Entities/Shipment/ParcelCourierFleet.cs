using CoreApplication.Domain.Common;
using CoreApplication.Domain.Enums;

namespace CoreApplication.Domain.Entities.Shipment;

public class ParcelCourierFleet : AuditableEntity
{
    public int ParcelCourierId { get; set; }
    public int FleetId { get; set; }
    public int SortOrder { get; set; }
    public ParcelTrackingEnum Status { get; set; }
    public bool IsDailySettlement { get; set; }
    public DateTime? DeliveredAt { get; set; }
    public string? ReceiverName { get; set; }
    public int? AuthType { get; set; }
    public string? Signature { get; set; }
    public string? PODCode { get; set; }
    public ReturnParcelReason? ReturnReason { get; set; }
    public string? ReturnNote { get; set; }

    public ParcelCourier ParcelCourier { get; set; } = default!;
    public Fleet.FleetEntity Fleet { get; set; } = default!;
    public ICollection<ParcelCourierFleetImage> Images { get; set; } = [];
}

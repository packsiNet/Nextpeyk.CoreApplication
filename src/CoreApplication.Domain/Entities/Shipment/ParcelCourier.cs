using CoreApplication.Domain.Common;
using CoreApplication.Domain.Enums;

namespace CoreApplication.Domain.Entities.Shipment;

public class ParcelCourier : AuditableEntity
{
    public int ParcelId { get; set; }
    public int CourierId { get; set; }
    public ServiceType ServiceType { get; set; }
    public int LNumber { get; set; }
    public ParcelTrackingEnum Status { get; set; }
    public string CollectFirstName { get; set; } = default!;
    public string CollectLastName { get; set; } = default!;
    public string CollectPhoneNumber { get; set; } = default!;
    public decimal CollectLatitude { get; set; }
    public decimal CollectLongitude { get; set; }
    public string CollectAddress { get; set; } = default!;
    public string DistributeFirstName { get; set; } = default!;
    public string DistributeLastName { get; set; } = default!;
    public string DistributePhoneNumber { get; set; } = default!;
    public decimal DistributeLatitude { get; set; }
    public decimal DistributeLongitude { get; set; }
    public string DistributeAddress { get; set; } = default!;
    public DateTime AssignedAt { get; set; }
    public DateTime PromisedDeliveryAt { get; set; }

    public Parcel Parcel { get; set; } = default!;
    public Courier.Courier Courier { get; set; } = default!;
    public ICollection<ParcelCourierFleet> ParcelCourierFleets { get; set; } = [];
    public ParcelCost? Cost { get; set; }
}

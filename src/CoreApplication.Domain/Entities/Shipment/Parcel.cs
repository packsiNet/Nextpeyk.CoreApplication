using CoreApplication.Domain.Common;
using CoreApplication.Domain.Enums;

namespace CoreApplication.Domain.Entities.Shipment;

public class Parcel : AuditableEntity
{
    public string Barcode { get; set; } = default!;
    public ParcelType ParcelType { get; set; }
    public string? Description { get; set; }
    public decimal Length { get; set; }
    public decimal Width { get; set; }
    public decimal Height { get; set; }
    public decimal Weight { get; set; }
    public decimal Value { get; set; }
    public string SenderFirstName { get; set; } = default!;
    public string SenderLastName { get; set; } = default!;
    public string SenderPhoneNumber { get; set; } = default!;
    public decimal SenderLatitude { get; set; }
    public decimal SenderLongitude { get; set; }
    public string SenderAddress { get; set; } = default!;
    public string ReceiverFirstName { get; set; } = default!;
    public string ReceiverLastName { get; set; } = default!;
    public string ReceiverPhoneNumber { get; set; } = default!;
    public decimal ReceiverLatitude { get; set; }
    public decimal ReceiverLongitude { get; set; }
    public string ReceiverAddress { get; set; } = default!;
    public bool HasCOD { get; set; }
    public bool HasFMCG { get; set; }
    public bool HasFreightCollect { get; set; }
    public ParcelState State { get; set; }
    public int SenderId { get; set; }

    public Sender.Sender Sender { get; set; } = default!;
    public ICollection<ParcelCourier> ParcelCouriers { get; set; } = [];
}

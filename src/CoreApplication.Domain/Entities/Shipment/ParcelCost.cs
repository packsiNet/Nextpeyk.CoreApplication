namespace CoreApplication.Domain.Entities.Shipment;

public class ParcelCost
{
    public int Id { get; set; }
    public int ParcelCourierId { get; set; }
    public decimal PickupCost { get; set; }
    public decimal DistributionCost { get; set; }
    public decimal TotalCost => PickupCost + DistributionCost;
    public DateTime CreatedAt { get; set; }
    public int CreatedByUserId { get; set; }

    public ParcelCourier ParcelCourier { get; set; } = default!;
}

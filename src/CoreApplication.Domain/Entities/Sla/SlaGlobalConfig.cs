namespace CoreApplication.Domain.Entities.Sla;

public class SlaGlobalConfig
{
    public int Id { get; set; }
    public decimal BenchmarkCostPerParcel { get; set; }
    public decimal WeightSuccessRate { get; set; }
    public decimal WeightOnTimeDelivery { get; set; }
    public decimal WeightReturnRate { get; set; }
    public decimal WeightDeliveryTime { get; set; }
    public decimal WeightCost { get; set; }
    public DateTime EffectiveFrom { get; set; }
    public int CreatedByUserId { get; set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<CarrierSlaSnapshot> Snapshots { get; set; } = [];
}

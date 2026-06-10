using CoreApplication.Domain.Enums;

namespace CoreApplication.Domain.Entities.Sla;

public class CarrierSlaSnapshot
{
    public int Id { get; set; }
    public int CourierId { get; set; }
    public int SlaGlobalConfigId { get; set; }
    public SlaPeriodType PeriodType { get; set; }
    public DateOnly PeriodStart { get; set; }
    public DateOnly PeriodEnd { get; set; }
    public int TotalAssigned { get; set; }
    public int TotalDelivered { get; set; }
    public int TotalOnTime { get; set; }
    public int TotalReturned { get; set; }
    public decimal? AvgDeliveryHours { get; set; }
    public decimal? AvgCostPerParcel { get; set; }
    public decimal SuccessRate { get; set; }
    public decimal OnTimeDelivery { get; set; }
    public decimal ReturnRate { get; set; }
    public decimal? DeliveryTimeScore { get; set; }
    public decimal? CostEfficiencyScore { get; set; }
    public decimal SlaScore { get; set; }
    public bool HasMissingKpi { get; set; }
    public int MissingKpiFlags { get; set; }
    public DateTime CreatedAt { get; set; }

    public Courier.Courier Courier { get; set; } = default!;
    public SlaGlobalConfig SlaGlobalConfig { get; set; } = default!;
    public ICollection<SlaAlert> Alerts { get; set; } = [];
}

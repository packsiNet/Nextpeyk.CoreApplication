using CoreApplication.Domain.Common;
using CoreApplication.Domain.Enums;

namespace CoreApplication.Domain.Entities.Courier;

public class CourierSlaConfig : BaseEntity
{
    public int CourierId { get; set; }
    public ServiceType ServiceType { get; set; }
    public int SlaHours { get; set; }
    public decimal SuccessRateMin { get; set; }
    public decimal OnTimeMin { get; set; }
    public decimal ReturnRateMax { get; set; }

    public Courier Courier { get; set; } = default!;
}

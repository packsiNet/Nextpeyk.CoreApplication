using CoreApplication.Domain.Common;

namespace CoreApplication.Domain.Entities.Courier;

public class CourierActivityTime : AuditableEntity
{
    public int CourierId { get; set; }
    public int DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }

    public Courier Courier { get; set; } = default!;
}

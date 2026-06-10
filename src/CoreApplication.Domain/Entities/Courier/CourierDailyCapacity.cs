namespace CoreApplication.Domain.Entities.Courier;

public class CourierDailyCapacity
{
    public int CourierId { get; set; }
    public DateOnly Date { get; set; }
    public int UsedCapacity { get; set; }

    public Courier Courier { get; set; } = default!;
}

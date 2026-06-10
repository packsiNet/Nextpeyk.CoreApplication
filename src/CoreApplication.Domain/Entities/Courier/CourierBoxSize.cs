using CoreApplication.Domain.Common;

namespace CoreApplication.Domain.Entities.Courier;

public class CourierBoxSize : AuditableEntity
{
    public int CourierId { get; set; }
    public int BoxSizeId { get; set; }
    public decimal PackagingPrice { get; set; }

    public Courier Courier { get; set; } = default!;
    public BoxSize BoxSize { get; set; } = default!;
}

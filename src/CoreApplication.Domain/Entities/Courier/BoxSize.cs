using CoreApplication.Domain.Common;

namespace CoreApplication.Domain.Entities.Courier;

public class BoxSize : AuditableEntity
{
    public int Order { get; set; }
    public string Title { get; set; } = default!;
    public string Code { get; set; } = default!;
    public decimal BoxLength { get; set; }
    public decimal BoxWidth { get; set; }
    public decimal BoxHeight { get; set; }

    public ICollection<CourierBoxSize> CourierBoxSizes { get; set; } = [];
}

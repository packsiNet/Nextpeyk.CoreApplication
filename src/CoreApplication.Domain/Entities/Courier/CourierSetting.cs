using CoreApplication.Domain.Common;

namespace CoreApplication.Domain.Entities.Courier;

public class CourierSetting : BaseEntity
{
    public int CourierId { get; set; }
    public bool IsGroupAcceptance { get; set; }

    public Courier Courier { get; set; } = default!;
}

namespace CoreApplication.Domain.Common;

public abstract class AuditableEntity : BaseEntity
{
    public string? CreatedByIp { get; set; }
    public byte[]? RowVersion { get; set; }
}

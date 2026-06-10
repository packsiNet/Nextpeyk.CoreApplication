using CoreApplication.Domain.Common;

namespace CoreApplication.Domain.Entities.Sender;

public class SenderApiCredential : BaseEntity
{
    public int SenderId { get; set; }
    public string ApiKey { get; set; } = default!;
    public string ApiSecretHash { get; set; } = default!;
    public string? Description { get; set; }
    public DateTime? ExpiresAt { get; set; }

    public Sender Sender { get; set; } = default!;
}

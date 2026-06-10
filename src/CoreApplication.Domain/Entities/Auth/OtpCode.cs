namespace CoreApplication.Domain.Entities.Auth;

public class OtpCode
{
    public int Id { get; set; }
    public string PhoneNumber { get; set; } = default!;
    public string Code { get; set; } = default!;
    public DateTime ExpiresAt { get; set; }
    public bool IsUsed { get; set; }
    public DateTime CreatedAt { get; set; }
}

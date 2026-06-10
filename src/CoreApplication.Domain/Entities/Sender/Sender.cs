using CoreApplication.Domain.Common;

namespace CoreApplication.Domain.Entities.Sender;

public class Sender : AuditableEntity
{
    public string Title { get; set; } = default!;
    public string? NationalCode { get; set; }
    public string? EconomicCode { get; set; }
    public string? RegistrationNumber { get; set; }
    public string? ContractNumber { get; set; }
    public DateOnly? ContractStartDate { get; set; }
    public DateOnly? ContractEndDate { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }

    public ICollection<SenderApiCredential> ApiCredentials { get; set; } = [];
    public ICollection<Auth.UserAccount> UserAccounts { get; set; } = [];
}

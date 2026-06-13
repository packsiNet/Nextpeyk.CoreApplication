using CoreApplication.Domain.Common;
using CoreApplication.Domain.Entities.Fleet;

namespace CoreApplication.Domain.Entities.Auth;

public class UserAccount : AuditableEntity
{
    public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string? PhoneNumber { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? NationalCode { get; set; }
    public string? SecurityStamp { get; set; }
    public bool MustChangePassword { get; set; } = false;
    public int? CourierId { get; set; }
    public int? SenderId { get; set; }

    public Courier.Courier? Courier { get; set; }
    public Sender.Sender? Sender { get; set; }
    public FleetEntity? Fleet { get; set; }
    public ICollection<UserRole> UserRoles { get; set; } = [];
}

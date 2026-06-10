using CoreApplication.Domain.Common;

namespace CoreApplication.Domain.Entities.Auth;

public class UserRole : BaseEntity
{
    public int UserAccountId { get; set; }
    public int RoleId { get; set; }

    public UserAccount UserAccount { get; set; } = default!;
    public Role Role { get; set; } = default!;
}

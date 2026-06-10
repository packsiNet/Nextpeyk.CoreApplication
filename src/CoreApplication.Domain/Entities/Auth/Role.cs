using CoreApplication.Domain.Common;

namespace CoreApplication.Domain.Entities.Auth;

public class Role : BaseEntity
{
    public string RoleName { get; set; } = default!;

    public ICollection<UserRole> UserRoles { get; set; } = [];
}

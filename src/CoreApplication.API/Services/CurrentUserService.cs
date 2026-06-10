using System.Security.Claims;
using CoreApplication.Application.Common.Interfaces;

namespace CoreApplication.API.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    private ClaimsPrincipal? User => httpContextAccessor.HttpContext?.User;

    public int? UserId
    {
        get
        {
            var value = User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return value is null ? null : int.Parse(value);
        }
    }

    public int? CourierId
    {
        get
        {
            var value = User?.FindFirstValue("CourierId");
            return value is null ? null : int.Parse(value);
        }
    }

    public string? UserName => User?.FindFirstValue(ClaimTypes.Name);

    public IEnumerable<string> Roles =>
        User?.FindAll(ClaimTypes.Role).Select(c => c.Value) ?? [];

    public bool IsInRole(string role) =>
        User?.IsInRole(role) ?? false;
}

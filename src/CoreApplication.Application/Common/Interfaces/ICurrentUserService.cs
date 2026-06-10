namespace CoreApplication.Application.Common.Interfaces;

public interface ICurrentUserService
{
    int? UserId { get; }
    int? CourierId { get; }
    string? UserName { get; }
    IEnumerable<string> Roles { get; }
    bool IsInRole(string role);
}

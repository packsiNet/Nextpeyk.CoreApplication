namespace CoreApplication.Application.Features.Auth.Common;

public record AuthResult(
    string Token,
    int UserId,
    string UserName,
    IEnumerable<string> Roles,
    int? CourierId);

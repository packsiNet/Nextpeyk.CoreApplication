using CoreApplication.Domain.Entities.Auth;

namespace CoreApplication.Application.Common.Interfaces;

public interface IJwtService
{
    string GenerateToken(UserAccount user, IEnumerable<string> roles);
    int? ValidateToken(string token);
}

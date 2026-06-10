using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Infrastructure.Auth;

namespace CoreApplication.API.Services;

public class SenderContext(IHttpContextAccessor httpContextAccessor) : ISenderContext
{
    public int SenderId
    {
        get
        {
            var value = httpContextAccessor.HttpContext?.User
                .FindFirst(ApiKeyDefaults.SenderIdClaimType)?.Value;

            if (value is null)
                throw new UnauthorizedAccessException("Sender context not available.");

            return int.Parse(value);
        }
    }
}

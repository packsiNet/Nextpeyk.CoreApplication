using System.Security.Claims;
using System.Text.Encodings.Web;
using CoreApplication.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CoreApplication.Infrastructure.Auth;

public static class ApiKeyDefaults
{
    public const string AuthenticationScheme = "ApiKey";
    public const string ApiKeyHeader = "X-Api-Key";
    public const string ApiSecretHeader = "X-Api-Secret";
    public const string SenderIdClaimType = "SenderId";
}

public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions { }

public class ApiKeyAuthenticationHandler(
    IOptionsMonitor<ApiKeyAuthenticationOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    IApplicationDbContext context,
    IPasswordService passwordService)
    : AuthenticationHandler<ApiKeyAuthenticationOptions>(options, logger, encoder)
{
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue(ApiKeyDefaults.ApiKeyHeader, out var apiKeyValues))
            return AuthenticateResult.NoResult();

        if (!Request.Headers.TryGetValue(ApiKeyDefaults.ApiSecretHeader, out var apiSecretValues))
            return AuthenticateResult.Fail("X-Api-Secret header is missing.");

        var apiKey = apiKeyValues.ToString();
        var apiSecret = apiSecretValues.ToString();

        var credential = await context.SenderApiCredentials
            .Include(c => c.Sender)
            .Where(c => c.ApiKey == apiKey && c.IsActive && !c.IsDeleted)
            .FirstOrDefaultAsync();

        if (credential is null)
            return AuthenticateResult.Fail("Invalid API key.");

        if (!passwordService.Verify(apiSecret, credential.ApiSecretHash))
            return AuthenticateResult.Fail("Invalid API secret.");

        if (credential.ExpiresAt.HasValue && credential.ExpiresAt < DateTime.UtcNow)
            return AuthenticateResult.Fail("API credential has expired.");

        if (!credential.Sender.IsActive || credential.Sender.IsDeleted)
            return AuthenticateResult.Fail("Sender account is inactive.");

        var claims = new[]
        {
            new Claim(ApiKeyDefaults.SenderIdClaimType, credential.SenderId.ToString()),
            new Claim(ClaimTypes.Name, credential.Sender.Title),
            new Claim(ClaimTypes.Role, "Sender")
        };

        var identity = new ClaimsIdentity(claims, ApiKeyDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, ApiKeyDefaults.AuthenticationScheme);

        return AuthenticateResult.Success(ticket);
    }
}

using CoreApplication.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace CoreApplication.Infrastructure.Services;

public class ConsoleSmsStub(ILogger<ConsoleSmsStub> logger) : ISmsSender
{
    public Task SendAsync(string phoneNumber, string message, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("[SMS STUB] To: {Phone} | Message: {Message}", phoneNumber, message);
        return Task.CompletedTask;
    }
}

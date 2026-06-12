using CoreApplication.Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CoreApplication.Infrastructure.Services;

public class SlaSnapshotBackgroundService(
    IServiceProvider serviceProvider,
    ILogger<SlaSnapshotBackgroundService> logger)
    : BackgroundService
{
    private static readonly TimeOnly RunAt = new(1, 0); // 01:00 AM daily

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("SLA snapshot background service started");

        while (!stoppingToken.IsCancellationRequested)
        {
            var delay = TimeUntilNextRun();
            logger.LogInformation("Next SLA snapshot run in {Minutes} minutes", (int)delay.TotalMinutes);

            await Task.Delay(delay, stoppingToken);

            if (stoppingToken.IsCancellationRequested) break;

            await RunCalculationAsync(stoppingToken);
        }
    }

    private async Task RunCalculationAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("SLA snapshot calculation started at {Time}", DateTime.UtcNow);

        try
        {
            using var scope = serviceProvider.CreateScope();
            var slaService = scope.ServiceProvider.GetRequiredService<ISlaCalculationService>();
            await slaService.RecalculateAllAsync(cancellationToken);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            logger.LogError(ex, "SLA snapshot calculation failed");
        }

        logger.LogInformation("SLA snapshot calculation finished at {Time}", DateTime.UtcNow);
    }

    private static TimeSpan TimeUntilNextRun()
    {
        var now = DateTime.Now;
        var nextRun = now.Date.Add(RunAt.ToTimeSpan());
        if (nextRun <= now)
            nextRun = nextRun.AddDays(1);
        return nextRun - now;
    }
}

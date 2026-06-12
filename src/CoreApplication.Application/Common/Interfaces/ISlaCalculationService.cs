namespace CoreApplication.Application.Common.Interfaces;

public interface ISlaCalculationService
{
    /// <summary>
    /// Recalculate all period snapshots for all active couriers.
    /// Called nightly by SlaSnapshotBackgroundService.
    /// Results are used by the Engine the following day.
    /// </summary>
    Task RecalculateAllAsync(CancellationToken cancellationToken = default);
}

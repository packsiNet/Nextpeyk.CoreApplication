namespace CoreApplication.Application.Common.Interfaces;

public record MatchedRoute(
    List<(double Lat, double Lon)> Points,
    double DistanceMeters,
    double DurationSeconds);

public interface IMapMatchingService
{
    Task<MatchedRoute?> MatchAsync(
        IReadOnlyList<(double Lat, double Lon, DateTime RecordedAt)> points,
        CancellationToken ct = default);
}

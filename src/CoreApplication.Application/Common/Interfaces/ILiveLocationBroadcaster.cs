namespace CoreApplication.Application.Common.Interfaces;

public record LiveLocationDto(
    int FleetId,
    int CourierId,
    string? DriverName,
    double Latitude,
    double Longitude,
    float? Speed,
    float? Heading,
    DateTime RecordedAt);

public interface ILiveLocationBroadcaster
{
    Task BroadcastAsync(LiveLocationDto dto, CancellationToken ct = default);
}

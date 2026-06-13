using System.Net.Http.Json;
using System.Text.Json;
using CoreApplication.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CoreApplication.Infrastructure.Services;

public class OsrmMapMatchingService(
    IHttpClientFactory httpClientFactory,
    IConfiguration configuration,
    ILogger<OsrmMapMatchingService> logger)
    : IMapMatchingService
{
    private const int MaxPointsPerRequest = 100;

    public async Task<MatchedRoute?> MatchAsync(
        IReadOnlyList<(double Lat, double Lon, DateTime RecordedAt)> points,
        CancellationToken ct = default)
    {
        if (points.Count < 2) return null;

        var baseUrl = configuration["Osrm:BaseUrl"];
        if (string.IsNullOrWhiteSpace(baseUrl)) return null;

        // OSRM limit: 100 points per request — sample evenly if needed
        var sampled = SamplePoints(points, MaxPointsPerRequest);

        var coords = string.Join(";", sampled.Select(p => $"{p.Lon:F6},{p.Lat:F6}"));
        var timestamps = string.Join(";", sampled.Select(p => new DateTimeOffset(p.RecordedAt, TimeSpan.Zero).ToUnixTimeSeconds()));

        var url = $"{baseUrl.TrimEnd('/')}/match/v1/driving/{coords}" +
                  $"?timestamps={timestamps}&geometries=geojson&overview=full&annotations=false";

        try
        {
            var client = httpClientFactory.CreateClient("osrm");
            var response = await client.GetAsync(url, ct);
            response.EnsureSuccessStatusCode();

            using var json = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync(ct), cancellationToken: ct);
            var root = json.RootElement;

            if (root.GetProperty("code").GetString() != "Ok") return null;

            var matchings = root.GetProperty("matchings");
            if (matchings.GetArrayLength() == 0) return null;

            var first = matchings[0];
            var distance = first.GetProperty("distance").GetDouble();
            var duration = first.GetProperty("duration").GetDouble();
            var geomCoords = first.GetProperty("geometry").GetProperty("coordinates");

            var resultPoints = new List<(double Lat, double Lon)>();
            foreach (var coord in geomCoords.EnumerateArray())
            {
                var lon = coord[0].GetDouble();
                var lat = coord[1].GetDouble();
                resultPoints.Add((lat, lon));
            }

            return new MatchedRoute(resultPoints, distance, duration);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "OSRM map matching failed for {Count} points", points.Count);
            return null;
        }
    }

    private static List<(double Lat, double Lon, DateTime RecordedAt)> SamplePoints(
        IReadOnlyList<(double Lat, double Lon, DateTime RecordedAt)> points,
        int maxCount)
    {
        if (points.Count <= maxCount) return points.ToList();

        var result = new List<(double, double, DateTime)>(maxCount);
        double step = (double)(points.Count - 1) / (maxCount - 1);

        for (int i = 0; i < maxCount; i++)
        {
            int idx = (int)Math.Round(i * step);
            result.Add(points[Math.Min(idx, points.Count - 1)]);
        }

        return result;
    }
}

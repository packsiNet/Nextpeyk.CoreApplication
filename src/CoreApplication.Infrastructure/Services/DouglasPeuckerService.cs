using CoreApplication.Application.Common.Interfaces;

namespace CoreApplication.Infrastructure.Services;

public class DouglasPeuckerService : IRouteSimplificationService
{
    public List<(double Lat, double Lon)> Simplify(
        IReadOnlyList<(double Lat, double Lon)> points,
        double toleranceMeters = 10.0)
    {
        if (points.Count <= 2) return points.ToList();

        var result = new List<(double Lat, double Lon)>();
        var keepFlags = new bool[points.Count];
        keepFlags[0] = true;
        keepFlags[^1] = true;

        DouglasPeucker(points, 0, points.Count - 1, toleranceMeters, keepFlags);

        for (int i = 0; i < points.Count; i++)
            if (keepFlags[i]) result.Add(points[i]);

        return result;
    }

    private static void DouglasPeucker(
        IReadOnlyList<(double Lat, double Lon)> points,
        int startIdx, int endIdx,
        double tolerance,
        bool[] keepFlags)
    {
        if (endIdx <= startIdx + 1) return;

        double maxDist = 0;
        int maxIdx = startIdx;

        for (int i = startIdx + 1; i < endIdx; i++)
        {
            double dist = PerpendicularDistanceMeters(points[i], points[startIdx], points[endIdx]);
            if (dist > maxDist)
            {
                maxDist = dist;
                maxIdx = i;
            }
        }

        if (maxDist > tolerance)
        {
            keepFlags[maxIdx] = true;
            DouglasPeucker(points, startIdx, maxIdx, tolerance, keepFlags);
            DouglasPeucker(points, maxIdx, endIdx, tolerance, keepFlags);
        }
    }

    private static double PerpendicularDistanceMeters(
        (double Lat, double Lon) point,
        (double Lat, double Lon) lineStart,
        (double Lat, double Lon) lineEnd)
    {
        double dLon = lineEnd.Lon - lineStart.Lon;
        double dLat = lineEnd.Lat - lineStart.Lat;

        if (dLon == 0 && dLat == 0)
            return HaversineMeters(point.Lat, point.Lon, lineStart.Lat, lineStart.Lon);

        double t = ((point.Lon - lineStart.Lon) * dLon + (point.Lat - lineStart.Lat) * dLat)
                 / (dLon * dLon + dLat * dLat);
        t = Math.Clamp(t, 0, 1);

        double closestLat = lineStart.Lat + t * dLat;
        double closestLon = lineStart.Lon + t * dLon;

        return HaversineMeters(point.Lat, point.Lon, closestLat, closestLon);
    }

    private static double HaversineMeters(double lat1, double lon1, double lat2, double lon2)
    {
        const double R = 6371000;
        var dLat = (lat2 - lat1) * Math.PI / 180;
        var dLon = (lon2 - lon1) * Math.PI / 180;
        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2)
              + Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180)
              * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        return R * 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
    }
}

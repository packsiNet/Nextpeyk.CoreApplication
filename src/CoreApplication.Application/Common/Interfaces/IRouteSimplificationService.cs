namespace CoreApplication.Application.Common.Interfaces;

public interface IRouteSimplificationService
{
    List<(double Lat, double Lon)> Simplify(
        IReadOnlyList<(double Lat, double Lon)> points,
        double toleranceMeters = 10.0);
}

using NetTopologySuite.Geometries;

namespace CoreApplication.Domain.Entities.Tracking;

public class CourierLocationTrack
{
    public int Id { get; set; }
    public int WorkSessionId { get; set; }
    public int FleetId { get; set; }
    public int CourierId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public Point Location { get; set; } = default!;
    public float? Accuracy { get; set; }
    public float? Speed { get; set; }
    public float? Heading { get; set; }
    public DateTime RecordedAt { get; set; }
    public DateTime CreatedAt { get; set; }

    public CourierWorkSession WorkSession { get; set; } = default!;
}

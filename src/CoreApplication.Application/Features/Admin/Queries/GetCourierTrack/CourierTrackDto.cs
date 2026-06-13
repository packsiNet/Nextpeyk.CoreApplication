namespace CoreApplication.Application.Features.Admin.Queries.GetCourierTrack;

public class TrackPointDto
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public float? Speed { get; set; }
    public float? Heading { get; set; }
    public float? Accuracy { get; set; }
    public DateTime RecordedAt { get; set; }
}

public class CourierTrackDto
{
    public int FleetId { get; set; }
    public int CourierId { get; set; }
    public DateOnly Date { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public double TotalDistanceMeters { get; set; }
    public List<TrackPointDto> Points { get; set; } = [];
    public List<TrackPointDto>? SimplifiedPoints { get; set; }
    public List<(double Lat, double Lon)>? MatchedRoute { get; set; }
}

namespace CoreApplication.Application.Features.Admin.Queries.GetActiveDriverLocations;

public class ActiveDriverLocationDto
{
    public int FleetId { get; set; }
    public int CourierId { get; set; }
    public string? DriverName { get; set; }
    public DateTime SessionStartedAt { get; set; }
    public double TotalDistanceMeters { get; set; }
    public double? LastLatitude { get; set; }
    public double? LastLongitude { get; set; }
    public float? LastSpeed { get; set; }
    public float? LastHeading { get; set; }
    public DateTime? LastSeenAt { get; set; }
}

using CoreApplication.Domain.Common;

namespace CoreApplication.Domain.Entities.Tracking;

public class CourierWorkSession : BaseEntity
{
    public int FleetId { get; set; }
    public int CourierId { get; set; }
    public DateOnly SessionDate { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public double TotalDistanceMeters { get; set; }

    public Fleet.FleetEntity Fleet { get; set; } = default!;
    public Courier.Courier Courier { get; set; } = default!;
    public ICollection<CourierLocationTrack> LocationTracks { get; set; } = [];
}

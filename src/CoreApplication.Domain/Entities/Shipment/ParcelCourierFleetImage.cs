using CoreApplication.Domain.Enums;

namespace CoreApplication.Domain.Entities.Shipment;

public class ParcelCourierFleetImage
{
    public int Id { get; set; }
    public int ParcelCourierFleetId { get; set; }
    public string ImagePath { get; set; } = default!;
    public ParcelTrackingEnum Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public int CreatedByUserId { get; set; }

    public ParcelCourierFleet ParcelCourierFleet { get; set; } = default!;
}

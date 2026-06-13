namespace CoreApplication.Domain.Enums;

public enum ParcelTrackingEnum
{
    Processing = 0,
    AssignToFleet = 1,
    ReceivedByFleet = 2,
    FinalizedDelivery = 3,
    Returned = 4,
    AssignCancelled = 5,
    PhysicallyAbsent = 6
}

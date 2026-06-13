namespace CoreApplication.Application.Features.CourierPanel.Common;

public record CourierDashboardDto(
    int Processing,
    int AssignedToFleet,
    int ReceivedByFleet,
    int DeliveredToday,
    int ReturnedToday,
    int ActiveFleets,
    int UnsettledCount);

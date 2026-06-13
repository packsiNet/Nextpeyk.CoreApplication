namespace CoreApplication.Application.Features.Admin.Common;

public record AdminDashboardStatsDto(
    ParcelStatsDto Parcels,
    CourierStatsDto Couriers,
    SenderStatsDto Senders,
    CodStatsDto Cod);

public record ParcelStatsDto(
    int TotalRegistered,
    int RegisteredToday,
    int InProgress,
    int PendingAssignment,
    int DeliveredToday,
    int ReturnedToday);

public record CourierStatsDto(
    int Total,
    int Active);

public record SenderStatsDto(
    int Total,
    int Active);

public record CodStatsDto(
    int PendingSettlement);

namespace CoreApplication.Application.Features.Couriers.Common;

public record CourierActivityTimeDto(
    int Id,
    int DayOfWeek,
    TimeOnly StartTime,
    TimeOnly EndTime);

using MediatR;

namespace CoreApplication.Application.Features.Couriers.Commands.SetCourierActivityTimes;

public record ActivityTimeItem(int DayOfWeek, TimeOnly StartTime, TimeOnly EndTime);

public record SetCourierActivityTimesCommand(
    int CourierId,
    List<ActivityTimeItem> ActivityTimes) : IRequest;

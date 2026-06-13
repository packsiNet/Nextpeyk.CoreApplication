using CoreApplication.Application.Features.CourierPanel.Common;
using CoreApplication.Domain.Enums;
using MediatR;

namespace CoreApplication.Application.Features.CourierPanel.Queries.GetCourierSlaTrend;

public record GetCourierSlaTrendQuery(
    SlaPeriodType PeriodType = SlaPeriodType.Weekly,
    int LastN = 12) : IRequest<List<CourierSlaTrendPointDto>>;

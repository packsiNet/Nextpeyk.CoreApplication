using CoreApplication.Application.Features.Admin.Reports.Common;
using CoreApplication.Domain.Enums;
using MediatR;

namespace CoreApplication.Application.Features.Admin.Reports.Queries.GetCourierTrend;

public record GetCourierTrendQuery(
    int CourierId,
    SlaPeriodType PeriodType = SlaPeriodType.Weekly,
    int LastN = 12) : IRequest<List<CourierTrendPointDto>>;

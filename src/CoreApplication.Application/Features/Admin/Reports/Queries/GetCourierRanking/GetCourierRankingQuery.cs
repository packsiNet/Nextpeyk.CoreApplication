using CoreApplication.Application.Features.Admin.Reports.Common;
using CoreApplication.Domain.Enums;
using MediatR;

namespace CoreApplication.Application.Features.Admin.Reports.Queries.GetCourierRanking;

public record GetCourierRankingQuery(
    SlaPeriodType PeriodType = SlaPeriodType.Rolling30) : IRequest<List<CourierRankingDto>>;

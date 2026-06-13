using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Application.Features.Admin.Reports.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.Admin.Reports.Queries.GetCourierRanking;

public class GetCourierRankingQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetCourierRankingQuery, List<CourierRankingDto>>
{
    public async Task<List<CourierRankingDto>> Handle(
        GetCourierRankingQuery request,
        CancellationToken cancellationToken)
    {
        // آخرین snapshot هر Courier برای این PeriodType
        var latestSnapshots = await context.CarrierSlaSnapshots
            .Where(s => s.PeriodType == request.PeriodType &&
                        s.PeriodStart == context.CarrierSlaSnapshots
                            .Where(inner => inner.CourierId == s.CourierId &&
                                           inner.PeriodType == request.PeriodType)
                            .Max(inner => inner.PeriodStart))
            .Include(s => s.Courier)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        // تعداد Alert های فعال هر Courier
        var alertCounts = await context.SlaAlerts
            .Where(a => !a.IsRead)
            .GroupBy(a => a.CourierId)
            .Select(g => new { CourierId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.CourierId, x => x.Count, cancellationToken);

        return latestSnapshots
            .OrderByDescending(s => s.SlaScore)
            .Select(s => new CourierRankingDto(
                s.CourierId,
                s.Courier.Title,
                s.SlaScore,
                s.SuccessRate,
                s.OnTimeDelivery,
                s.ReturnRate,
                s.AvgDeliveryHours,
                s.TotalAssigned,
                s.TotalDelivered,
                s.PeriodStart,
                s.PeriodEnd,
                alertCounts.GetValueOrDefault(s.CourierId, 0),
                s.HasMissingKpi))
            .ToList();
    }
}

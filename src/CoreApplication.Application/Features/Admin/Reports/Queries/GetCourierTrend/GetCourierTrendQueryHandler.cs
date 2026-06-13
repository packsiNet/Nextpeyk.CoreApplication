using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Application.Features.Admin.Reports.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.Admin.Reports.Queries.GetCourierTrend;

public class GetCourierTrendQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetCourierTrendQuery, List<CourierTrendPointDto>>
{
    public async Task<List<CourierTrendPointDto>> Handle(
        GetCourierTrendQuery request,
        CancellationToken cancellationToken)
    {
        return await context.CarrierSlaSnapshots
            .Where(s => s.CourierId == request.CourierId &&
                        s.PeriodType == request.PeriodType)
            .OrderByDescending(s => s.PeriodStart)
            .Take(request.LastN)
            .Select(s => new CourierTrendPointDto(
                s.PeriodStart,
                s.PeriodEnd,
                s.SlaScore,
                s.SuccessRate,
                s.OnTimeDelivery,
                s.ReturnRate,
                s.AvgDeliveryHours,
                s.TotalAssigned,
                s.TotalDelivered,
                s.TotalReturned))
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}

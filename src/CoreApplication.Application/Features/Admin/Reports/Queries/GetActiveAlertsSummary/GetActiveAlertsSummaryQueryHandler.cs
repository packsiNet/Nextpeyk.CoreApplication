using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Application.Features.Admin.Reports.Common;
using CoreApplication.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.Admin.Reports.Queries.GetActiveAlertsSummary;

public class GetActiveAlertsSummaryQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetActiveAlertsSummaryQuery, List<ActiveAlertSummaryDto>>
{
    public async Task<List<ActiveAlertSummaryDto>> Handle(
        GetActiveAlertsSummaryQuery request,
        CancellationToken cancellationToken)
    {
        var alerts = await context.SlaAlerts
            .Where(a => !a.IsRead)
            .Include(a => a.Courier)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return alerts
            .GroupBy(a => a.CourierId)
            .Select(g => new ActiveAlertSummaryDto(
                g.Key,
                g.First().Courier.Title,
                g.Count(),
                g.Select(a => a.AlertType).Distinct().ToList()))
            .OrderByDescending(x => x.AlertCount)
            .ToList();
    }
}

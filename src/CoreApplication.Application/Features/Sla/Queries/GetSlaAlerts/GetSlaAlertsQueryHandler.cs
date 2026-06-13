using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Application.Common.Models;
using CoreApplication.Application.Features.Sla.Common;
using MediatR;

namespace CoreApplication.Application.Features.Sla.Queries.GetSlaAlerts;

public class GetSlaAlertsQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetSlaAlertsQuery, PaginatedList<SlaAlertDto>>
{
    public async Task<PaginatedList<SlaAlertDto>> Handle(GetSlaAlertsQuery request, CancellationToken cancellationToken)
    {
        var query = context.SlaAlerts.AsQueryable();

        if (request.CourierId.HasValue)
            query = query.Where(a => a.CourierId == request.CourierId.Value);

        if (request.IsRead.HasValue)
            query = query.Where(a => a.IsRead == request.IsRead.Value);

        var projected = query
            .OrderByDescending(a => a.CreatedAt)
            .Select(a => new SlaAlertDto(
                a.Id, a.CourierId, a.Courier.Title,
                a.SnapshotId, a.Snapshot.PeriodType, a.Snapshot.PeriodStart,
                a.AlertType, a.IsRead, a.CreatedAt));

        return await PaginatedList<SlaAlertDto>.CreateAsync(projected, request.PageNumber, request.PageSize, cancellationToken);
    }
}

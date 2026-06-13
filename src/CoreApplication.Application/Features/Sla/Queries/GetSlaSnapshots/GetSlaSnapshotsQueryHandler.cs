using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Application.Common.Models;
using CoreApplication.Application.Features.Sla.Common;
using MediatR;

namespace CoreApplication.Application.Features.Sla.Queries.GetSlaSnapshots;

public class GetSlaSnapshotsQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetSlaSnapshotsQuery, PaginatedList<SlaSnapshotDto>>
{
    public async Task<PaginatedList<SlaSnapshotDto>> Handle(GetSlaSnapshotsQuery request, CancellationToken cancellationToken)
    {
        var query = context.CarrierSlaSnapshots
            .AsQueryable();

        if (request.CourierId.HasValue)
            query = query.Where(s => s.CourierId == request.CourierId.Value);

        if (request.PeriodType.HasValue)
            query = query.Where(s => s.PeriodType == request.PeriodType.Value);

        if (request.From.HasValue)
            query = query.Where(s => s.PeriodStart >= request.From.Value);

        if (request.To.HasValue)
            query = query.Where(s => s.PeriodEnd <= request.To.Value);

        var projected = query
            .OrderByDescending(s => s.CreatedAt)
            .Select(s => new SlaSnapshotDto(
                s.Id, s.CourierId, s.Courier.Title,
                s.PeriodType, s.PeriodStart, s.PeriodEnd,
                s.TotalAssigned, s.TotalDelivered, s.TotalOnTime, s.TotalReturned,
                s.AvgDeliveryHours, s.AvgCostPerParcel,
                s.SuccessRate, s.OnTimeDelivery, s.ReturnRate,
                s.DeliveryTimeScore, s.CostEfficiencyScore,
                s.SlaScore, s.HasMissingKpi, s.CreatedAt));

        return await PaginatedList<SlaSnapshotDto>.CreateAsync(projected, request.PageNumber, request.PageSize, cancellationToken);
    }
}

using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Application.Features.Admin.Reports.Common;
using CoreApplication.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.Admin.Reports.Queries.GetReturnReasonBreakdown;

public class GetReturnReasonBreakdownQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetReturnReasonBreakdownQuery, List<ReturnReasonBreakdownDto>>
{
    public async Task<List<ReturnReasonBreakdownDto>> Handle(
        GetReturnReasonBreakdownQuery request,
        CancellationToken cancellationToken)
    {
        var query = context.ParcelCourierFleets
            .Where(f => !f.IsDeleted &&
                        f.Status == ParcelTrackingEnum.Returned &&
                        f.ReturnReason != null);

        if (request.CourierId.HasValue)
            query = query.Where(f => f.ParcelCourier.CourierId == request.CourierId.Value);

        if (request.From.HasValue)
            query = query.Where(f => DateOnly.FromDateTime(f.CreatedAt) >= request.From.Value);

        if (request.To.HasValue)
            query = query.Where(f => DateOnly.FromDateTime(f.CreatedAt) <= request.To.Value);

        var grouped = await query
            .GroupBy(f => f.ReturnReason!.Value)
            .Select(g => new { Reason = g.Key, Count = g.Count() })
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var total = grouped.Sum(x => x.Count);
        if (total == 0) return [];

        return grouped
            .OrderByDescending(x => x.Count)
            .Select(x => new ReturnReasonBreakdownDto(
                x.Reason,
                x.Count,
                total > 0 ? Math.Round((decimal)x.Count / total * 100, 1) : 0))
            .ToList();
    }
}

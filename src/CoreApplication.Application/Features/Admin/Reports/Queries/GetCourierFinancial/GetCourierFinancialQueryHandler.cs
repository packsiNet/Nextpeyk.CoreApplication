using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Application.Features.Admin.Reports.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.Admin.Reports.Queries.GetCourierFinancial;

public class GetCourierFinancialQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetCourierFinancialQuery, List<CourierFinancialDto>>
{
    public async Task<List<CourierFinancialDto>> Handle(
        GetCourierFinancialQuery request,
        CancellationToken cancellationToken)
    {
        var query = context.ParcelCosts
            .Include(pc => pc.ParcelCourier)
                .ThenInclude(pcr => pcr.Courier)
            .AsQueryable();

        if (request.CourierId.HasValue)
            query = query.Where(pc => pc.ParcelCourier.CourierId == request.CourierId.Value);

        if (request.From.HasValue)
            query = query.Where(pc => DateOnly.FromDateTime(pc.CreatedAt) >= request.From.Value);

        if (request.To.HasValue)
            query = query.Where(pc => DateOnly.FromDateTime(pc.CreatedAt) <= request.To.Value);

        var raw = await query
            .GroupBy(pc => new { pc.ParcelCourier.CourierId, CourierTitle = pc.ParcelCourier.Courier.Title })
            .Select(g => new
            {
                g.Key.CourierId,
                g.Key.CourierTitle,
                TotalParcels = g.Count(),
                PickupTotal = g.Sum(x => x.PickupCost),
                DistributionTotal = g.Sum(x => x.DistributionCost),
                TotalRevenue = g.Sum(x => x.PickupCost + x.DistributionCost)
            })
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return raw
            .Select(x => new CourierFinancialDto(
                x.CourierId,
                x.CourierTitle,
                x.TotalParcels,
                x.TotalRevenue,
                x.TotalParcels > 0 ? Math.Round(x.TotalRevenue / x.TotalParcels, 0) : 0,
                x.PickupTotal,
                x.DistributionTotal))
            .OrderByDescending(x => x.TotalRevenue)
            .ToList();
    }
}

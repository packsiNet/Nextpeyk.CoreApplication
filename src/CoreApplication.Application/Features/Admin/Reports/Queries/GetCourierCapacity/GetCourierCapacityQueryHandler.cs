using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Application.Features.Admin.Reports.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.Admin.Reports.Queries.GetCourierCapacity;

public class GetCourierCapacityQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetCourierCapacityQuery, List<CourierCapacityDto>>
{
    public async Task<List<CourierCapacityDto>> Handle(
        GetCourierCapacityQuery request,
        CancellationToken cancellationToken)
    {
        var capacityQuery = context.CourierDailyCapacities.AsQueryable();

        if (request.From.HasValue)
            capacityQuery = capacityQuery.Where(c => c.Date >= request.From.Value);

        if (request.To.HasValue)
            capacityQuery = capacityQuery.Where(c => c.Date <= request.To.Value);

        var grouped = await capacityQuery
            .GroupBy(c => c.CourierId)
            .Select(g => new
            {
                CourierId = g.Key,
                AvgLoad = g.Average(c => (double)c.UsedCapacity),
                PeakLoad = g.Max(c => c.UsedCapacity)
            })
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        if (grouped.Count == 0) return [];

        var courierIds = grouped.Select(x => x.CourierId).ToList();
        var couriers = await context.Couriers
            .Where(c => courierIds.Contains(c.Id))
            .Select(c => new { c.Id, c.Title, c.MaximumCapacityPerDay })
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var courierMap = couriers.ToDictionary(c => c.Id);

        return grouped
            .Where(g => courierMap.ContainsKey(g.CourierId))
            .Select(g =>
            {
                var courier = courierMap[g.CourierId];
                var avgLoad = (decimal)g.AvgLoad;
                var loadPercent = courier.MaximumCapacityPerDay > 0
                    ? Math.Round(avgLoad / courier.MaximumCapacityPerDay * 100, 1)
                    : 0;
                return new CourierCapacityDto(
                    g.CourierId,
                    courier.Title,
                    courier.MaximumCapacityPerDay,
                    Math.Round(avgLoad, 1),
                    g.PeakLoad,
                    loadPercent);
            })
            .OrderByDescending(x => x.LoadPercent)
            .ToList();
    }
}

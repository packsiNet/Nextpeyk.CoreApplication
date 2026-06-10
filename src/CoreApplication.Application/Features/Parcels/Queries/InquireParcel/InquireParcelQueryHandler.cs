using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Application.Features.Parcels.Common;
using CoreApplication.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace CoreApplication.Application.Features.Parcels.Queries.InquireParcel;

public class InquireParcelQueryHandler(IApplicationDbContext context)
    : IRequestHandler<InquireParcelQuery, List<CourierOptionDto>>
{
    public async Task<List<CourierOptionDto>> Handle(InquireParcelQuery request, CancellationToken cancellationToken)
    {
        var receiverPoint = new Point(request.ReceiverLongitude, request.ReceiverLatitude) { SRID = 4326 };

        var matchedZones = await context.CourierZones
            .Where(z =>
                z.ZoneType == ZoneType.Distribution &&
                !z.IsDeleted &&
                z.Courier.IsActive && !z.Courier.IsDeleted &&
                z.Courier.MaximumShipmentWeight >= request.Weight &&
                (z.Courier.HasCOD || !request.HasCOD) &&
                (z.Courier.HasFMCG || !request.HasFMCG) &&
                (z.Courier.HasFreightCollect || !request.HasFreightCollect) &&
                z.Courier.SlaConfigs.Any(s => s.ServiceType == ServiceType.LastMile && !s.IsDeleted) &&
                (
                    (z.Boundary != null && z.Boundary.Contains(receiverPoint)) ||
                    (z.CenterPoint != null && z.CenterPoint.Distance(receiverPoint) <= (double)z.Radius * 1000)
                )
            )
            .Include(z => z.Courier)
                .ThenInclude(c => c.SlaConfigs)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        // یک Courier ممکنه چند zone داشته باشه — بهترین zone (ارزان‌ترین) رو انتخاب میکنیم
        return matchedZones
            .GroupBy(z => z.CourierId)
            .Select(g =>
            {
                var zone = g.OrderBy(z => z.Price ?? decimal.MaxValue).First();
                var courier = zone.Courier;
                var slaConfig = courier.SlaConfigs
                    .Where(s => s.ServiceType == ServiceType.LastMile && !s.IsDeleted)
                    .OrderBy(s => s.SlaHours)
                    .First();

                var distributionPrice = zone.Price ?? courier.BasePrice;
                var extras = (request.HasCOD ? courier.CodPrice : 0)
                           + (request.HasFreightCollect ? courier.FreightCollectPrice : 0);
                var total = distributionPrice + extras;

                return new CourierOptionDto(
                    courier.Id,
                    courier.Title,
                    courier.Logo,
                    courier.SupportPhoneNumber,
                    slaConfig.SlaHours,
                    courier.BasePrice,
                    zone.Price,
                    total);
            })
            .OrderBy(o => o.SlaHours)
            .ThenBy(o => o.EstimatedTotal)
            .ToList();
    }
}

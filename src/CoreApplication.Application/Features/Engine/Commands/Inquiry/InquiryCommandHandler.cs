using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Application.Features.Engine.Common;
using CoreApplication.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace CoreApplication.Application.Features.Engine.Commands.Inquiry;

public class InquiryCommandHandler(IApplicationDbContext context, IDateTimeService dateTime)
    : IRequestHandler<InquiryCommand, InquiryResponseDto>
{
    public async Task<InquiryResponseDto> Handle(InquiryCommand request, CancellationToken cancellationToken)
    {
        var today = DateOnly.FromDateTime(dateTime.UtcNow);

        var dailyCapacities = await context.CourierDailyCapacities
            .Where(dc => dc.Date == today)
            .ToDictionaryAsync(dc => dc.CourierId, dc => dc.UsedCapacity, cancellationToken);

        // Load latest Rolling30 SLA snapshot per courier (calculated by previous night's job)
        var slaSnapshots = await context.CarrierSlaSnapshots
            .Where(s => s.PeriodType == SlaPeriodType.Rolling30)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var latestSnapshot = slaSnapshots
            .GroupBy(s => s.CourierId)
            .ToDictionary(g => g.Key, g => g.OrderByDescending(s => s.PeriodStart).First());

        var capacityAdjustments = new Dictionary<int, int>();
        var acceptedByCourier = new Dictionary<int, (Domain.Entities.Courier.Courier Courier, List<AcceptedShipmentDetailDto> Details)>();
        var rejected = new List<RejectedShipmentDetailDto>();

        foreach (var shipment in request.Shipments)
        {
            var collectionPoint = new Point(shipment.Collection.Longitude, shipment.Collection.Latitude) { SRID = 4326 };
            var distributionPoint = new Point(shipment.Distribution.Longitude, shipment.Distribution.Latitude) { SRID = 4326 };

            var collectionCourierIds = await context.CourierZones
                .Where(z =>
                    z.ZoneType == ZoneType.Collection &&
                    z.IsActive && !z.IsDeleted &&
                    z.Courier.IsActive && !z.Courier.IsDeleted &&
                    ((z.Boundary != null && z.Boundary.Contains(collectionPoint)) ||
                     (z.CenterPoint != null && z.CenterPoint.Distance(collectionPoint) <= (double)z.Radius * 1000)))
                .Select(z => z.CourierId)
                .Distinct()
                .ToListAsync(cancellationToken);

            var distributionCourierIds = await context.CourierZones
                .Where(z =>
                    z.ZoneType == ZoneType.Distribution &&
                    z.IsActive && !z.IsDeleted &&
                    z.Courier.IsActive && !z.Courier.IsDeleted &&
                    ((z.Boundary != null && z.Boundary.Contains(distributionPoint)) ||
                     (z.CenterPoint != null && z.CenterPoint.Distance(distributionPoint) <= (double)z.Radius * 1000)))
                .Select(z => z.CourierId)
                .Distinct()
                .ToListAsync(cancellationToken);

            var eligibleIds = collectionCourierIds.Intersect(distributionCourierIds).ToHashSet();

            if (eligibleIds.Count == 0)
            {
                rejected.Add(new RejectedShipmentDetailDto(shipment.LNumber, "No courier covers both zones"));
                continue;
            }

            var candidates = await context.Couriers
                .Where(c =>
                    eligibleIds.Contains(c.Id) &&
                    c.MaximumShipmentWeight >= request.Weight &&
                    (!shipment.HasCOD || c.HasCOD) &&
                    (!shipment.HasFMCG || c.HasFMCG) &&
                    (!shipment.HasFreightCollect || c.HasFreightCollect) &&
                    c.SlaConfigs.Any(s => s.ServiceType == shipment.ServiceType && !s.IsDeleted))
                .Include(c => c.SlaConfigs.Where(s => !s.IsDeleted))
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var assigned = candidates
                .Where(c =>
                {
                    var usedToday = dailyCapacities.GetValueOrDefault(c.Id, 0);
                    var thisRequest = capacityAdjustments.GetValueOrDefault(c.Id, 0);
                    return (usedToday + thisRequest) < c.MaximumCapacityPerDay;
                })
                // Primary: highest SlaScore from yesterday's snapshot (real performance)
                // Fallback: lowest SlaHours from static config (new/unscored couriers go last)
                .OrderByDescending(c => latestSnapshot.TryGetValue(c.Id, out var snap) ? snap.SlaScore : -1m)
                .ThenBy(c => c.SlaConfigs
                    .Where(s => s.ServiceType == shipment.ServiceType)
                    .Min(s => s.SlaHours))
                .FirstOrDefault();

            if (assigned == null)
            {
                rejected.Add(new RejectedShipmentDetailDto(shipment.LNumber, "No courier has available capacity"));
                continue;
            }

            capacityAdjustments[assigned.Id] = capacityAdjustments.GetValueOrDefault(assigned.Id, 0) + 1;

            var slaHours = assigned.SlaConfigs
                .Where(s => s.ServiceType == shipment.ServiceType)
                .Min(s => s.SlaHours);

            var price = assigned.BasePrice
                + (shipment.HasCOD ? assigned.CodPrice : 0)
                + (shipment.HasFreightCollect ? assigned.FreightCollectPrice : 0);

            if (!acceptedByCourier.TryGetValue(assigned.Id, out var entry))
            {
                entry = (assigned, []);
                acceptedByCourier[assigned.Id] = entry;
            }
            entry.Details.Add(new AcceptedShipmentDetailDto(shipment.LNumber, slaHours, price));
        }

        return new InquiryResponseDto(
            acceptedByCourier.Values
                .Select(e => new AcceptedCourierDto(e.Courier.Id, e.Courier.Title, e.Details.Count, e.Details))
                .ToList(),
            new RejectedItemsDto(rejected.Count, rejected));
    }
}

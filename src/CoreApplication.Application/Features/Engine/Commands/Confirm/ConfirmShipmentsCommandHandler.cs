using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Application.Features.Engine.Common;
using CoreApplication.Domain.Entities.Courier;
using CoreApplication.Domain.Entities.Shipment;
using CoreApplication.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace CoreApplication.Application.Features.Engine.Commands.Confirm;

public class ConfirmShipmentsCommandHandler(
    IApplicationDbContext context,
    ISenderContext senderContext,
    ICurrentUserService currentUser,
    IDateTimeService dateTime)
    : IRequestHandler<ConfirmShipmentsCommand, ConfirmResponseDto>
{
    public async Task<ConfirmResponseDto> Handle(ConfirmShipmentsCommand request, CancellationToken cancellationToken)
    {
        var now = dateTime.UtcNow;
        var today = DateOnly.FromDateTime(now);
        var userId = currentUser.UserId ?? 0;
        var senderId = senderContext.SenderId;

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

        // Phase 1: match each shipment to best courier (same logic as Inquiry)
        var assignments = new List<(ConfirmShipmentItemDto Shipment, Domain.Entities.Courier.Courier Courier, int SlaHours, decimal Price)>();
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

            assignments.Add((shipment, assigned, slaHours, price));
        }

        // Phase 2: persist all accepted assignments
        var acceptedByCourier = new Dictionary<int, (Domain.Entities.Courier.Courier Courier, List<ConfirmedShipmentDetailDto> Details)>();

        foreach (var (shipment, courier, slaHours, price) in assignments)
        {
            var barcode = GenerateBarcode(now);

            var parcel = new Parcel
            {
                Barcode = barcode,
                ParcelType = shipment.ParcelType,
                Description = shipment.Description,
                Length = request.Length,
                Width = request.Width,
                Height = request.Height,
                Weight = request.Weight,
                Value = shipment.Value,
                SenderFirstName = shipment.CollectFirstName,
                SenderLastName = shipment.CollectLastName,
                SenderPhoneNumber = shipment.CollectPhoneNumber,
                SenderLatitude = (decimal)shipment.Collection.Latitude,
                SenderLongitude = (decimal)shipment.Collection.Longitude,
                SenderAddress = shipment.CollectAddress,
                ReceiverFirstName = shipment.DistributeFirstName,
                ReceiverLastName = shipment.DistributeLastName,
                ReceiverPhoneNumber = shipment.DistributePhoneNumber,
                ReceiverLatitude = (decimal)shipment.Distribution.Latitude,
                ReceiverLongitude = (decimal)shipment.Distribution.Longitude,
                ReceiverAddress = shipment.DistributeAddress,
                HasCOD = shipment.HasCOD,
                HasFMCG = shipment.HasFMCG,
                HasFreightCollect = shipment.HasFreightCollect,
                State = ParcelState.Registered,
                SenderId = senderId,
                CreatedByUserId = userId,
                ModifiedByUserId = userId,
                ModifiedDateTime = now,
                CreatedAt = now,
                IsActive = true
            };

            context.Parcels.Add(parcel);
            await context.SaveChangesAsync(cancellationToken);

            var parcelCourier = new ParcelCourier
            {
                ParcelId = parcel.Id,
                CourierId = courier.Id,
                ServiceType = shipment.ServiceType,
                LNumber = shipment.LNumber,
                Status = ParcelTrackingEnum.Processing,
                CollectFirstName = shipment.CollectFirstName,
                CollectLastName = shipment.CollectLastName,
                CollectPhoneNumber = shipment.CollectPhoneNumber,
                CollectLatitude = (decimal)shipment.Collection.Latitude,
                CollectLongitude = (decimal)shipment.Collection.Longitude,
                CollectAddress = shipment.CollectAddress,
                DistributeFirstName = shipment.DistributeFirstName,
                DistributeLastName = shipment.DistributeLastName,
                DistributePhoneNumber = shipment.DistributePhoneNumber,
                DistributeLatitude = (decimal)shipment.Distribution.Latitude,
                DistributeLongitude = (decimal)shipment.Distribution.Longitude,
                DistributeAddress = shipment.DistributeAddress,
                AssignedAt = now,
                PromisedDeliveryAt = now.AddHours(slaHours),
                CreatedByUserId = userId,
                ModifiedByUserId = userId,
                ModifiedDateTime = now,
                CreatedAt = now,
                IsActive = true
            };

            context.ParcelCouriers.Add(parcelCourier);
            await context.SaveChangesAsync(cancellationToken);

            context.ParcelCosts.Add(new ParcelCost
            {
                ParcelCourierId = parcelCourier.Id,
                PickupCost = 0,
                DistributionCost = price,
                CreatedAt = now,
                CreatedByUserId = userId
            });

            if (!acceptedByCourier.TryGetValue(courier.Id, out var entry))
            {
                entry = (courier, []);
                acceptedByCourier[courier.Id] = entry;
            }
            entry.Details.Add(new ConfirmedShipmentDetailDto(shipment.LNumber, parcel.Id, barcode, slaHours, price));
        }

        // Phase 3: upsert daily capacity
        foreach (var (courierId, count) in capacityAdjustments)
        {
            var cap = await context.CourierDailyCapacities
                .FirstOrDefaultAsync(dc => dc.CourierId == courierId && dc.Date == today, cancellationToken);

            if (cap == null)
            {
                context.CourierDailyCapacities.Add(new CourierDailyCapacity
                {
                    CourierId = courierId,
                    Date = today,
                    UsedCapacity = count
                });
            }
            else
            {
                cap.UsedCapacity += count;
            }
        }

        await context.SaveChangesAsync(cancellationToken);

        return new ConfirmResponseDto(
            acceptedByCourier.Values
                .Select(e => new ConfirmedCourierDto(e.Courier.Id, e.Courier.Title, e.Details.Count, e.Details))
                .ToList(),
            new RejectedItemsDto(rejected.Count, rejected));
    }

    private static string GenerateBarcode(DateTime now)
        => $"PKG-{now:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..8].ToUpper()}";
}

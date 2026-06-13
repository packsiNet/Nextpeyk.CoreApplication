using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Application.Features.Admin.Common;
using CoreApplication.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.Admin.Queries.GetAdminDashboardStats;

public class GetAdminDashboardStatsQueryHandler(
    IApplicationDbContext context,
    IDateTimeService dateTime)
    : IRequestHandler<GetAdminDashboardStatsQuery, AdminDashboardStatsDto>
{
    public async Task<AdminDashboardStatsDto> Handle(
        GetAdminDashboardStatsQuery request,
        CancellationToken cancellationToken)
    {
        var today = DateOnly.FromDateTime(dateTime.UtcNow);
        var todayStart = today.ToDateTime(TimeOnly.MinValue);
        var todayEnd = today.ToDateTime(TimeOnly.MaxValue);

        var inProgressStatuses = new[]
        {
            ParcelTrackingEnum.Processing,
            ParcelTrackingEnum.AssignToFleet,
            ParcelTrackingEnum.ReceivedByFleet
        };

        // --- Parcels ---
        var parcelsBase = context.Parcels.Where(p => !p.IsDeleted);

        var totalRegistered = await parcelsBase.CountAsync(cancellationToken);

        var registeredToday = await parcelsBase
            .CountAsync(p => p.CreatedAt >= todayStart && p.CreatedAt <= todayEnd, cancellationToken);

        var inProgress = await context.ParcelCouriers
            .Where(pc => !pc.IsDeleted && inProgressStatuses.Contains(pc.Status))
            .CountAsync(cancellationToken);

        var pendingAssignment = await context.ParcelCouriers
            .Where(pc => !pc.IsDeleted && pc.Status == ParcelTrackingEnum.Processing)
            .CountAsync(cancellationToken);

        var deliveredToday = await context.ParcelCourierFleets
            .Where(f => !f.IsDeleted &&
                        f.Status == ParcelTrackingEnum.FinalizedDelivery &&
                        f.DeliveredAt >= todayStart && f.DeliveredAt <= todayEnd)
            .CountAsync(cancellationToken);

        var returnedToday = await context.ParcelCourierFleets
            .Where(f => !f.IsDeleted &&
                        f.Status == ParcelTrackingEnum.Returned &&
                        f.CreatedAt >= todayStart && f.CreatedAt <= todayEnd)
            .CountAsync(cancellationToken);

        // --- Couriers ---
        var totalCouriers = await context.Couriers
            .CountAsync(c => !c.IsDeleted, cancellationToken);

        var activeCouriers = await context.Couriers
            .CountAsync(c => !c.IsDeleted && c.IsActive, cancellationToken);

        // --- Senders ---
        var totalSenders = await context.Senders
            .CountAsync(s => !s.IsDeleted, cancellationToken);

        var activeSenders = await context.Senders
            .CountAsync(s => !s.IsDeleted && s.IsActive, cancellationToken);

        // --- COD pending settlement ---
        var codPending = await context.ParcelCourierFleets
            .Where(f => !f.IsDeleted &&
                        f.Status == ParcelTrackingEnum.FinalizedDelivery &&
                        !f.IsDailySettlement &&
                        f.ParcelCourier.Parcel.HasCOD)
            .CountAsync(cancellationToken);

        return new AdminDashboardStatsDto(
            new ParcelStatsDto(totalRegistered, registeredToday, inProgress, pendingAssignment, deliveredToday, returnedToday),
            new CourierStatsDto(totalCouriers, activeCouriers),
            new SenderStatsDto(totalSenders, activeSenders),
            new CodStatsDto(codPending));
    }
}

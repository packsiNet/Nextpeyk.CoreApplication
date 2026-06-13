using CoreApplication.Application.Common.Exceptions;
using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Application.Features.CourierPanel.Common;
using CoreApplication.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.CourierPanel.Queries.GetCourierDashboard;

public class GetCourierDashboardQueryHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser)
    : IRequestHandler<GetCourierDashboardQuery, CourierDashboardDto>
{
    public async Task<CourierDashboardDto> Handle(
        GetCourierDashboardQuery request,
        CancellationToken cancellationToken)
    {
        var courierId = currentUser.CourierId ?? throw new ForbiddenAccessException();

        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);

        var processingTask = context.ParcelCouriers.CountAsync(
            pc => pc.CourierId == courierId &&
                  pc.Status == ParcelTrackingEnum.Processing &&
                  !pc.IsDeleted, cancellationToken);

        var assignedTask = context.ParcelCouriers.CountAsync(
            pc => pc.CourierId == courierId &&
                  pc.Status == ParcelTrackingEnum.AssignToFleet &&
                  !pc.IsDeleted, cancellationToken);

        var receivedTask = context.ParcelCouriers.CountAsync(
            pc => pc.CourierId == courierId &&
                  pc.Status == ParcelTrackingEnum.ReceivedByFleet &&
                  !pc.IsDeleted, cancellationToken);

        var deliveredTodayTask = context.ParcelCourierFleets.CountAsync(
            f => f.ParcelCourier.CourierId == courierId &&
                 f.Status == ParcelTrackingEnum.FinalizedDelivery &&
                 f.DeliveredAt >= today && f.DeliveredAt < tomorrow &&
                 !f.IsDeleted, cancellationToken);

        var returnedTodayTask = context.ParcelCourierFleets.CountAsync(
            f => f.ParcelCourier.CourierId == courierId &&
                 f.Status == ParcelTrackingEnum.Returned &&
                 f.CreatedAt >= today && f.CreatedAt < tomorrow &&
                 !f.IsDeleted, cancellationToken);

        var activeFleetsTask = context.Fleets.CountAsync(
            f => f.CourierId == courierId && f.IsActive && !f.IsDeleted,
            cancellationToken);

        var unsettledTask = context.ParcelCourierFleets.CountAsync(
            f => f.Fleet.CourierId == courierId &&
                 !f.IsDailySettlement &&
                 (f.Status == ParcelTrackingEnum.FinalizedDelivery ||
                  f.Status == ParcelTrackingEnum.Returned) &&
                 !f.IsDeleted, cancellationToken);

        await Task.WhenAll(
            processingTask, assignedTask, receivedTask,
            deliveredTodayTask, returnedTodayTask,
            activeFleetsTask, unsettledTask);

        return new CourierDashboardDto(
            await processingTask,
            await assignedTask,
            await receivedTask,
            await deliveredTodayTask,
            await returnedTodayTask,
            await activeFleetsTask,
            await unsettledTask);
    }
}

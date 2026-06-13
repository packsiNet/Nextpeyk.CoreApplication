using CoreApplication.Application.Common.Exceptions;
using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Application.Features.CourierPanel.Common;
using CoreApplication.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.CourierPanel.Queries.GetCourierSummaryReport;

public class GetCourierSummaryReportQueryHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser)
    : IRequestHandler<GetCourierSummaryReportQuery, CourierSummaryReportDto>
{
    public async Task<CourierSummaryReportDto> Handle(
        GetCourierSummaryReportQuery request,
        CancellationToken cancellationToken)
    {
        var courierId = currentUser.CourierId ?? throw new ForbiddenAccessException();

        var fromDt = request.From.ToDateTime(TimeOnly.MinValue);
        var toDt = request.To.ToDateTime(TimeOnly.MaxValue);

        // Base: all parcels assigned to this courier in the date range
        var baseQuery = context.ParcelCouriers
            .Where(pc =>
                pc.CourierId == courierId &&
                !pc.IsDeleted &&
                pc.AssignedAt >= fromDt &&
                pc.AssignedAt <= toDt);

        var totalTask = baseQuery.CountAsync(cancellationToken);

        var absentTask = baseQuery.CountAsync(
            pc => pc.Status == ParcelTrackingEnum.PhysicallyAbsent,
            cancellationToken);

        var deliveredTask = baseQuery.CountAsync(
            pc => pc.ParcelCourierFleets.Any(
                f => f.Status == ParcelTrackingEnum.FinalizedDelivery && !f.IsDeleted),
            cancellationToken);

        // Returned: has a fleet record with Returned status
        var returnedTask = baseQuery.CountAsync(
            pc => pc.ParcelCourierFleets.Any(
                f => f.Status == ParcelTrackingEnum.Returned && !f.IsDeleted),
            cancellationToken);

        // Valid returns: returned AND the fleet record has at least one image
        var validReturnsTask = baseQuery.CountAsync(
            pc => pc.ParcelCourierFleets.Any(
                f => f.Status == ParcelTrackingEnum.Returned &&
                     !f.IsDeleted &&
                     f.Images.Any()),
            cancellationToken);

        await Task.WhenAll(totalTask, absentTask, deliveredTask, returnedTask, validReturnsTask);

        var total = await totalTask;
        var absent = await absentTask;
        var returned = await returnedTask;
        var validReturns = await validReturnsTask;

        return new CourierSummaryReportDto(
            request.From,
            request.To,
            Total: total,
            PhysicallyAbsent: absent,
            Confirmed: total - absent,
            TotalReturned: returned,
            ValidReturns: validReturns,
            InvalidReturns: returned - validReturns,
            Delivered: await deliveredTask);
    }
}

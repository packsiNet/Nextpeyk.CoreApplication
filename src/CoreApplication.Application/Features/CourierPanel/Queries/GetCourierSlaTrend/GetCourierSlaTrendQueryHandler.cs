using CoreApplication.Application.Common.Exceptions;
using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Application.Features.CourierPanel.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.CourierPanel.Queries.GetCourierSlaTrend;

public class GetCourierSlaTrendQueryHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser)
    : IRequestHandler<GetCourierSlaTrendQuery, List<CourierSlaTrendPointDto>>
{
    public async Task<List<CourierSlaTrendPointDto>> Handle(
        GetCourierSlaTrendQuery request,
        CancellationToken cancellationToken)
    {
        var courierId = currentUser.CourierId ?? throw new ForbiddenAccessException();

        return await context.CarrierSlaSnapshots
            .Where(s => s.CourierId == courierId && s.PeriodType == request.PeriodType)
            .OrderByDescending(s => s.PeriodStart)
            .Take(request.LastN)
            .Select(s => new CourierSlaTrendPointDto(
                s.PeriodStart,
                s.PeriodEnd,
                s.PeriodType,
                s.SlaScore,
                s.SuccessRate,
                s.OnTimeDelivery,
                s.ReturnRate,
                s.AvgDeliveryHours,
                s.TotalAssigned,
                s.TotalDelivered,
                s.TotalReturned))
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}

using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Application.Features.Couriers.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.Couriers.Queries.GetCourierSetting;

public class GetCourierSettingQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetCourierSettingQuery, CourierSettingDto?>
{
    public async Task<CourierSettingDto?> Handle(GetCourierSettingQuery request, CancellationToken cancellationToken)
    {
        var setting = await context.CourierSettings
            .Where(s => s.CourierId == request.CourierId && !s.IsDeleted)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        return setting is null
            ? null
            : new CourierSettingDto(setting.Id, setting.IsGroupAcceptance);
    }
}

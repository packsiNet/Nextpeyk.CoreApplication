using CoreApplication.Application.Common.Exceptions;
using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Application.Features.Couriers.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.Couriers.Queries.GetCourierById;

public class GetCourierByIdQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetCourierByIdQuery, CourierDetailDto>
{
    public async Task<CourierDetailDto> Handle(GetCourierByIdQuery request, CancellationToken cancellationToken)
    {
        var courier = await context.Couriers
            .Include(c => c.Setting)
            .Include(c => c.Zones.Where(z => !z.IsDeleted))
                .ThenInclude(z => z.City)
            .Include(c => c.ActivityTimes.Where(a => !a.IsDeleted))
            .Include(c => c.BoxSizes.Where(b => !b.IsDeleted))
                .ThenInclude(b => b.BoxSize)
            .Include(c => c.SlaConfigs.Where(s => !s.IsDeleted))
            .Where(c => c.Id == request.Id && !c.IsDeleted)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Courier.Courier), request.Id);

        return new CourierDetailDto(
            courier.Id,
            courier.Title,
            courier.Description,
            courier.Logo,
            courier.SupportPhoneNumber,
            courier.SupportFullName,
            courier.Website,
            courier.SystemName,
            courier.EconomicCode,
            courier.NationalCode,
            courier.BasePrice,
            courier.CodPrice,
            courier.FreightCollectPrice,
            courier.MaximumCapacityPerDay,
            courier.MaximumShipmentWeight,
            courier.MaximumValueOfTheShipment,
            courier.MinimumParcelsInOneOrder,
            courier.HasCOD,
            courier.HasFMCG,
            courier.HasFreightCollect,
            courier.HasPackaging,
            courier.IsActive,
            courier.Setting is null ? null : new CourierSettingDto(courier.Setting.Id, courier.Setting.IsGroupAcceptance),
            courier.Zones.Select(z => new CourierZoneDto(
                z.Id, z.ZoneType, z.Name, z.CityId,
                z.City?.Name, z.Boundary, z.CenterPoint, z.Radius, z.Price)).ToList(),
            courier.ActivityTimes.Select(a => new CourierActivityTimeDto(
                a.Id, a.DayOfWeek, a.StartTime, a.EndTime)).ToList(),
            courier.BoxSizes.Select(b => new CourierBoxSizeDto(
                b.Id, b.BoxSizeId, b.BoxSize.Title, b.BoxSize.Code, b.PackagingPrice)).ToList(),
            courier.SlaConfigs.Select(s => new CourierSlaConfigDto(
                s.Id, s.ServiceType, s.SlaHours, s.SuccessRateMin, s.OnTimeMin, s.ReturnRateMax)).ToList());
    }
}

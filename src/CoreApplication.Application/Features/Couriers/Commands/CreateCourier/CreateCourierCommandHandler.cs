using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Domain.Entities.Courier;
using MediatR;

namespace CoreApplication.Application.Features.Couriers.Commands.CreateCourier;

public class CreateCourierCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser,
    IDateTimeService dateTime)
    : IRequestHandler<CreateCourierCommand, int>
{
    public async Task<int> Handle(CreateCourierCommand request, CancellationToken cancellationToken)
    {
        var now = dateTime.UtcNow;
        var userId = currentUser.UserId ?? 0;

        var courier = new Domain.Entities.Courier.Courier
        {
            Title = request.Title,
            Description = request.Description,
            Logo = request.Logo,
            SupportPhoneNumber = request.SupportPhoneNumber,
            SupportFullName = request.SupportFullName,
            Website = request.Website,
            SystemName = request.SystemName,
            EconomicCode = request.EconomicCode,
            NationalCode = request.NationalCode,
            BasePrice = request.BasePrice,
            CodPrice = request.CodPrice,
            FreightCollectPrice = request.FreightCollectPrice,
            MaximumCapacityPerDay = request.MaximumCapacityPerDay,
            MaximumShipmentWeight = request.MaximumShipmentWeight,
            MaximumValueOfTheShipment = request.MaximumValueOfTheShipment,
            MinimumParcelsInOneOrder = request.MinimumParcelsInOneOrder,
            HasCOD = request.HasCOD,
            HasFMCG = request.HasFMCG,
            HasFreightCollect = request.HasFreightCollect,
            HasPackaging = request.HasPackaging,
            CreatedByUserId = userId,
            ModifiedByUserId = userId,
            ModifiedDateTime = now,
            CreatedAt = now,
            IsActive = true
        };

        context.Couriers.Add(courier);
        await context.SaveChangesAsync(cancellationToken);

        var setting = new CourierSetting
        {
            CourierId = courier.Id,
            IsGroupAcceptance = request.IsGroupAcceptance,
            CreatedByUserId = userId,
            ModifiedByUserId = userId,
            ModifiedDateTime = now,
            CreatedAt = now,
            IsActive = true
        };

        context.CourierSettings.Add(setting);
        await context.SaveChangesAsync(cancellationToken);

        return courier.Id;
    }
}

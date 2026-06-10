using CoreApplication.Application.Common.Exceptions;
using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Domain.Entities.Courier;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.Couriers.Commands.UpdateCourier;

public class UpdateCourierCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser,
    IDateTimeService dateTime)
    : IRequestHandler<UpdateCourierCommand>
{
    public async Task Handle(UpdateCourierCommand request, CancellationToken cancellationToken)
    {
        var courier = await context.Couriers
            .Include(c => c.Setting)
            .Where(c => c.Id == request.Id && !c.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Courier.Courier), request.Id);

        var now = dateTime.UtcNow;
        var userId = currentUser.UserId ?? 0;

        courier.Title = request.Title;
        courier.Description = request.Description;
        courier.Logo = request.Logo;
        courier.SupportPhoneNumber = request.SupportPhoneNumber;
        courier.SupportFullName = request.SupportFullName;
        courier.Website = request.Website;
        courier.SystemName = request.SystemName;
        courier.EconomicCode = request.EconomicCode;
        courier.NationalCode = request.NationalCode;
        courier.BasePrice = request.BasePrice;
        courier.CodPrice = request.CodPrice;
        courier.FreightCollectPrice = request.FreightCollectPrice;
        courier.MaximumCapacityPerDay = request.MaximumCapacityPerDay;
        courier.MaximumShipmentWeight = request.MaximumShipmentWeight;
        courier.MaximumValueOfTheShipment = request.MaximumValueOfTheShipment;
        courier.MinimumParcelsInOneOrder = request.MinimumParcelsInOneOrder;
        courier.HasCOD = request.HasCOD;
        courier.HasFMCG = request.HasFMCG;
        courier.HasFreightCollect = request.HasFreightCollect;
        courier.HasPackaging = request.HasPackaging;
        courier.ModifiedByUserId = userId;
        courier.ModifiedDateTime = now;

        if (courier.Setting is not null)
        {
            courier.Setting.IsGroupAcceptance = request.IsGroupAcceptance;
            courier.Setting.ModifiedByUserId = userId;
            courier.Setting.ModifiedDateTime = now;
        }
        else
        {
            context.CourierSettings.Add(new CourierSetting
            {
                CourierId = courier.Id,
                IsGroupAcceptance = request.IsGroupAcceptance,
                CreatedByUserId = userId,
                ModifiedByUserId = userId,
                ModifiedDateTime = now,
                CreatedAt = now,
                IsActive = true
            });
        }

        await context.SaveChangesAsync(cancellationToken);
    }
}

using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Domain.Entities.Auth;
using CoreApplication.Domain.Entities.Courier;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.Couriers.Commands.CreateCourier;

public class CreateCourierCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser,
    IPasswordService passwordService,
    IDateTimeService dateTime)
    : IRequestHandler<CreateCourierCommand, int>
{
    private const int CourierManagerRoleId = 2;

    public async Task<int> Handle(CreateCourierCommand request, CancellationToken cancellationToken)
    {
        var now = dateTime.UtcNow;
        var userId = currentUser.UserId ?? 0;

        var userNameTaken = await context.UserAccounts
            .AnyAsync(u => u.UserName == request.UserName && !u.IsDeleted, cancellationToken);
        if (userNameTaken)
            throw new InvalidOperationException($"Username '{request.UserName}' is already taken.");

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

        var managerAccount = new UserAccount
        {
            UserName = request.UserName,
            Password = passwordService.Hash(request.Password),
            PhoneNumber = request.UserPhoneNumber,
            FirstName = request.UserFirstName,
            LastName = request.UserLastName,
            SecurityStamp = Guid.NewGuid().ToString(),
            MustChangePassword = request.ForcePasswordChange,
            CourierId = courier.Id,
            CreatedByUserId = userId,
            ModifiedByUserId = userId,
            ModifiedDateTime = now,
            CreatedAt = now,
            IsActive = true
        };

        context.UserAccounts.Add(managerAccount);
        await context.SaveChangesAsync(cancellationToken);

        context.UserRoles.Add(new UserRole
        {
            UserAccountId = managerAccount.Id,
            RoleId = CourierManagerRoleId,
            CreatedByUserId = userId,
            ModifiedByUserId = userId,
            CreatedAt = now,
            IsActive = true
        });

        await context.SaveChangesAsync(cancellationToken);

        return courier.Id;
    }
}

using CoreApplication.Application.Common.Exceptions;
using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Domain.Entities.Auth;
using CoreApplication.Domain.Entities.Fleet;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.Fleets.Commands.CreateFleet;

public class CreateFleetCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser,
    IPasswordService passwordService,
    IDateTimeService dateTime)
    : IRequestHandler<CreateFleetCommand, int>
{
    private const int DriverRoleId = 4;

    public async Task<int> Handle(CreateFleetCommand request, CancellationToken cancellationToken)
    {
        var courierId = currentUser.CourierId
            ?? throw new ForbiddenAccessException();

        var now = dateTime.UtcNow;
        var userId = currentUser.UserId ?? 0;

        // Validate FleetType
        var fleetTypeExists = await context.FleetTypes
            .AnyAsync(ft => ft.Id == request.FleetTypeId && ft.IsActive && !ft.IsDeleted, cancellationToken);
        if (!fleetTypeExists)
            throw new NotFoundException("FleetType", request.FleetTypeId);

        // Validate username uniqueness
        var userNameTaken = await context.UserAccounts
            .AnyAsync(u => u.UserName == request.UserName && !u.IsDeleted, cancellationToken);
        if (userNameTaken)
            throw new InvalidOperationException($"Username '{request.UserName}' is already taken.");

        // Create driver UserAccount
        var driverAccount = new UserAccount
        {
            UserName = request.UserName,
            Password = passwordService.Hash(request.Password),
            PhoneNumber = request.PhoneNumber,
            FirstName = request.FirstName,
            LastName = request.LastName,
            NationalCode = request.NationalCode,
            SecurityStamp = Guid.NewGuid().ToString(),
            CourierId = courierId,
            CreatedByUserId = userId,
            ModifiedByUserId = userId,
            ModifiedDateTime = now,
            CreatedAt = now,
            IsActive = true
        };

        context.UserAccounts.Add(driverAccount);
        await context.SaveChangesAsync(cancellationToken);

        // Assign Driver role
        context.UserRoles.Add(new UserRole
        {
            UserAccountId = driverAccount.Id,
            RoleId = DriverRoleId,
            CreatedAt = now,
            IsActive = true
        });

        // Create Fleet
        var fleet = new FleetEntity
        {
            UserAccountId = driverAccount.Id,
            CourierId = courierId,
            FleetTypeId = request.FleetTypeId,
            Plaque = request.Plaque,
            DrivingLicense = request.DrivingLicense,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Description = request.Description,
            InsuranceCode = request.InsuranceCode,
            CreatedByUserId = userId,
            ModifiedByUserId = userId,
            ModifiedDateTime = now,
            CreatedAt = now,
            IsActive = true
        };

        context.Fleets.Add(fleet);
        await context.SaveChangesAsync(cancellationToken);

        return fleet.Id;
    }
}

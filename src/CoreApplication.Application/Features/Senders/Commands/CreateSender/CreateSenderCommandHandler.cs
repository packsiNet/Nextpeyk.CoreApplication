using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Domain.Entities.Auth;
using CoreApplication.Domain.Entities.Sender;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.Senders.Commands.CreateSender;

public class CreateSenderCommandHandler(
    IApplicationDbContext context,
    IPasswordService passwordService,
    ICurrentUserService currentUser,
    IDateTimeService dateTime)
    : IRequestHandler<CreateSenderCommand, int>
{
    public async Task<int> Handle(CreateSenderCommand request, CancellationToken cancellationToken)
    {
        var senderRole = await context.Roles
            .FirstAsync(r => r.RoleName == "Sender" && r.IsActive, cancellationToken);

        var sender = new Domain.Entities.Sender.Sender
        {
            Title = request.Title,
            NationalCode = request.NationalCode,
            EconomicCode = request.EconomicCode,
            RegistrationNumber = request.RegistrationNumber,
            ContractNumber = request.ContractNumber,
            ContractStartDate = request.ContractStartDate,
            ContractEndDate = request.ContractEndDate,
            Address = request.Address,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            CreatedByUserId = currentUser.UserId ?? 0,
            ModifiedByUserId = currentUser.UserId ?? 0,
            ModifiedDateTime = dateTime.UtcNow,
            CreatedAt = dateTime.UtcNow,
            IsActive = true
        };

        context.Senders.Add(sender);
        await context.SaveChangesAsync(cancellationToken);

        var userAccount = new UserAccount
        {
            UserName = request.UserName,
            Password = passwordService.Hash(request.Password),
            PhoneNumber = request.UserPhoneNumber,
            SenderId = sender.Id,
            CreatedByUserId = currentUser.UserId ?? 0,
            ModifiedByUserId = currentUser.UserId ?? 0,
            ModifiedDateTime = dateTime.UtcNow,
            CreatedAt = dateTime.UtcNow,
            IsActive = true
        };

        context.UserAccounts.Add(userAccount);
        await context.SaveChangesAsync(cancellationToken);

        context.UserRoles.Add(new UserRole
        {
            UserAccountId = userAccount.Id,
            RoleId = senderRole.Id,
            CreatedByUserId = currentUser.UserId ?? 0,
            ModifiedByUserId = currentUser.UserId ?? 0,
            CreatedAt = dateTime.UtcNow,
            IsActive = true
        });

        await context.SaveChangesAsync(cancellationToken);

        return sender.Id;
    }
}

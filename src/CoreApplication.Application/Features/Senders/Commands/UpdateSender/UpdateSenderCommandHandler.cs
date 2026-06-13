using CoreApplication.Application.Common.Exceptions;
using CoreApplication.Application.Common.Interfaces;
using MediatR;

namespace CoreApplication.Application.Features.Senders.Commands.UpdateSender;

public class UpdateSenderCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser,
    IDateTimeService dateTime)
    : IRequestHandler<UpdateSenderCommand>
{
    public async Task Handle(UpdateSenderCommand request, CancellationToken cancellationToken)
    {
        var sender = await context.Senders.FindAsync([request.Id], cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Sender.Sender), request.Id);

        if (sender.IsDeleted)
            throw new NotFoundException(nameof(Domain.Entities.Sender.Sender), request.Id);

        sender.Title = request.Title;
        sender.NationalCode = request.NationalCode;
        sender.EconomicCode = request.EconomicCode;
        sender.RegistrationNumber = request.RegistrationNumber;
        sender.ContractNumber = request.ContractNumber;
        sender.ContractStartDate = request.ContractStartDate;
        sender.ContractEndDate = request.ContractEndDate;
        sender.Address = request.Address;
        sender.PhoneNumber = request.PhoneNumber;
        sender.Email = request.Email;
        sender.ModifiedByUserId = currentUser.UserId ?? 0;
        sender.ModifiedDateTime = dateTime.UtcNow;

        await context.SaveChangesAsync(cancellationToken);
    }
}

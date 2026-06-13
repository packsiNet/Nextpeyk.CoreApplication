using CoreApplication.Application.Common.Exceptions;
using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Application.Features.Senders.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.Senders.Queries.GetSenderById;

public class GetSenderByIdQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetSenderByIdQuery, SenderDetailDto>
{
    public async Task<SenderDetailDto> Handle(GetSenderByIdQuery request, CancellationToken cancellationToken)
    {
        var sender = await context.Senders
            .Include(s => s.ApiCredentials.Where(c => !c.IsDeleted))
            .Where(s => s.Id == request.Id && !s.IsDeleted)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Sender.Sender), request.Id);

        return new SenderDetailDto(
            sender.Id,
            sender.Title,
            sender.NationalCode,
            sender.EconomicCode,
            sender.RegistrationNumber,
            sender.ContractNumber,
            sender.ContractStartDate,
            sender.ContractEndDate,
            sender.Address,
            sender.PhoneNumber,
            sender.Email,
            sender.IsActive,
            sender.ApiCredentials.Select(c => new SenderCredentialDto(
                c.Id, c.ApiKey, c.Description, c.ExpiresAt, c.IsActive)).ToList());
    }
}

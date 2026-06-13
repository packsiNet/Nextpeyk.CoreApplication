using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Application.Common.Models;
using CoreApplication.Application.Features.Senders.Common;
using MediatR;

namespace CoreApplication.Application.Features.Senders.Queries.GetSenders;

public class GetSendersQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetSendersQuery, PaginatedList<SenderListDto>>
{
    public async Task<PaginatedList<SenderListDto>> Handle(GetSendersQuery request, CancellationToken cancellationToken)
    {
        var query = context.Senders
            .Where(s => !s.IsDeleted)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            query = query.Where(s =>
                s.Title.Contains(request.SearchTerm) ||
                (s.PhoneNumber != null && s.PhoneNumber.Contains(request.SearchTerm)) ||
                (s.ContractNumber != null && s.ContractNumber.Contains(request.SearchTerm)));

        if (request.IsActive.HasValue)
            query = query.Where(s => s.IsActive == request.IsActive.Value);

        var projected = query
            .OrderBy(s => s.Title)
            .Select(s => new SenderListDto(
                s.Id, s.Title, s.ContractNumber,
                s.ContractStartDate, s.ContractEndDate,
                s.PhoneNumber, s.Email, s.IsActive));

        return await PaginatedList<SenderListDto>.CreateAsync(projected, request.PageNumber, request.PageSize, cancellationToken);
    }
}

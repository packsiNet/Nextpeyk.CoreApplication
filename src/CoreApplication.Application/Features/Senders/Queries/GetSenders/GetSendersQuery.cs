using CoreApplication.Application.Common.Models;
using CoreApplication.Application.Features.Senders.Common;
using MediatR;

namespace CoreApplication.Application.Features.Senders.Queries.GetSenders;

public record GetSendersQuery(
    int PageNumber = 1,
    int PageSize = 20,
    string? SearchTerm = null,
    bool? IsActive = null) : IRequest<PaginatedList<SenderListDto>>;

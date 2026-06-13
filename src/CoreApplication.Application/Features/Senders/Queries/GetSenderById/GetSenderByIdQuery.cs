using CoreApplication.Application.Features.Senders.Common;
using MediatR;

namespace CoreApplication.Application.Features.Senders.Queries.GetSenderById;

public record GetSenderByIdQuery(int Id) : IRequest<SenderDetailDto>;

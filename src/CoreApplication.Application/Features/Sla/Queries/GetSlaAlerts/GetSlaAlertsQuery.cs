using CoreApplication.Application.Common.Models;
using CoreApplication.Application.Features.Sla.Common;
using MediatR;

namespace CoreApplication.Application.Features.Sla.Queries.GetSlaAlerts;

public record GetSlaAlertsQuery(
    int PageNumber = 1,
    int PageSize = 20,
    int? CourierId = null,
    bool? IsRead = null) : IRequest<PaginatedList<SlaAlertDto>>;

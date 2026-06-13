using CoreApplication.Application.Features.Admin.Reports.Common;
using MediatR;

namespace CoreApplication.Application.Features.Admin.Reports.Queries.GetReturnReasonBreakdown;

public record GetReturnReasonBreakdownQuery(
    int? CourierId = null,
    DateOnly? From = null,
    DateOnly? To = null) : IRequest<List<ReturnReasonBreakdownDto>>;

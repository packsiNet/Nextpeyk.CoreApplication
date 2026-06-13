using CoreApplication.Application.Features.Admin.Reports.Common;
using MediatR;

namespace CoreApplication.Application.Features.Admin.Reports.Queries.GetActiveAlertsSummary;

public record GetActiveAlertsSummaryQuery : IRequest<List<ActiveAlertSummaryDto>>;

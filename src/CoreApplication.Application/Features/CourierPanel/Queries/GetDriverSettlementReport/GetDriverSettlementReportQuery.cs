using CoreApplication.Application.Features.CourierPanel.Common;
using MediatR;

namespace CoreApplication.Application.Features.CourierPanel.Queries.GetDriverSettlementReport;

public record GetDriverSettlementReportQuery(
    DateOnly Date) : IRequest<List<DriverSettlementReportDto>>;

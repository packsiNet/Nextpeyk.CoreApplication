using CoreApplication.Application.Features.CourierPanel.Common;
using MediatR;

namespace CoreApplication.Application.Features.CourierPanel.Queries.GetFleetDailyReport;

public record GetFleetDailyReportQuery(
    DateOnly? Date = null) : IRequest<List<FleetDailyReportDto>>;

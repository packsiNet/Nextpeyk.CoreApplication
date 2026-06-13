using CoreApplication.Application.Features.CourierPanel.Common;
using MediatR;

namespace CoreApplication.Application.Features.CourierPanel.Queries.GetCourierSummaryReport;

public record GetCourierSummaryReportQuery(
    DateOnly From,
    DateOnly To) : IRequest<CourierSummaryReportDto>;

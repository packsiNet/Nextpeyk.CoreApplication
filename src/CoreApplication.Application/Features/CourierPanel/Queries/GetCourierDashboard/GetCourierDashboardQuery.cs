using CoreApplication.Application.Features.CourierPanel.Common;
using MediatR;

namespace CoreApplication.Application.Features.CourierPanel.Queries.GetCourierDashboard;

public record GetCourierDashboardQuery : IRequest<CourierDashboardDto>;

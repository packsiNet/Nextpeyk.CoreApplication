using CoreApplication.Application.Features.Admin.Common;
using MediatR;

namespace CoreApplication.Application.Features.Admin.Queries.GetAdminDashboardStats;

public record GetAdminDashboardStatsQuery : IRequest<AdminDashboardStatsDto>;

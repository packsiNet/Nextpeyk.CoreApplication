using CoreApplication.Application.Common.Models;
using CoreApplication.Application.Features.Couriers.Common;
using MediatR;

namespace CoreApplication.Application.Features.Couriers.Queries.GetCouriers;

public record GetCouriersQuery(
    int PageNumber = 1,
    int PageSize = 20,
    string? SearchTerm = null,
    bool? IsActive = null) : IRequest<PaginatedList<CourierListDto>>;

using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Application.Common.Models;
using CoreApplication.Application.Features.Couriers.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.Couriers.Queries.GetCouriers;

public class GetCouriersQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetCouriersQuery, PaginatedList<CourierListDto>>
{
    public async Task<PaginatedList<CourierListDto>> Handle(GetCouriersQuery request, CancellationToken cancellationToken)
    {
        var query = context.Couriers
            .Where(c => !c.IsDeleted)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            query = query.Where(c => c.Title.Contains(request.SearchTerm) || (c.SystemName != null && c.SystemName.Contains(request.SearchTerm)));

        if (request.IsActive.HasValue)
            query = query.Where(c => c.IsActive == request.IsActive.Value);

        var projected = query
            .OrderBy(c => c.Title)
            .Select(c => new CourierListDto(
                c.Id, c.Title, c.Description, c.Logo,
                c.SupportPhoneNumber, c.HasCOD, c.HasFMCG,
                c.HasFreightCollect, c.HasPackaging, c.IsActive));

        return await PaginatedList<CourierListDto>.CreateAsync(projected, request.PageNumber, request.PageSize, cancellationToken);
    }
}

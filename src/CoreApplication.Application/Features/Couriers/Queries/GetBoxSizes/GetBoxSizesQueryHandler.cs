using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Application.Features.Couriers.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.Couriers.Queries.GetBoxSizes;

public class GetBoxSizesQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetBoxSizesQuery, List<BoxSizeDto>>
{
    public async Task<List<BoxSizeDto>> Handle(GetBoxSizesQuery request, CancellationToken cancellationToken)
    {
        return await context.BoxSizes
            .Where(b => !b.IsDeleted)
            .OrderBy(b => b.Order)
            .Select(b => new BoxSizeDto(b.Id, b.Order, b.Title, b.Code, b.BoxLength, b.BoxWidth, b.BoxHeight))
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}

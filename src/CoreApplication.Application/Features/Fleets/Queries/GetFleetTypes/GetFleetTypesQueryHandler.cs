using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Application.Features.Fleets.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.Fleets.Queries.GetFleetTypes;

public class GetFleetTypesQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetFleetTypesQuery, List<FleetTypeDto>>
{
    public async Task<List<FleetTypeDto>> Handle(GetFleetTypesQuery request, CancellationToken cancellationToken)
        => await context.FleetTypes
            .Where(ft => ft.IsActive && !ft.IsDeleted)
            .Select(ft => new FleetTypeDto(ft.Id, ft.Title))
            .AsNoTracking()
            .ToListAsync(cancellationToken);
}

using CoreApplication.Application.Common.Exceptions;
using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Application.Features.Fleets.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.Fleets.Queries.GetFleets;

public class GetFleetsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    : IRequestHandler<GetFleetsQuery, List<FleetListDto>>
{
    public async Task<List<FleetListDto>> Handle(GetFleetsQuery request, CancellationToken cancellationToken)
    {
        var courierId = currentUser.CourierId
            ?? throw new ForbiddenAccessException();

        var query = context.Fleets
            .Where(f => f.CourierId == courierId && !f.IsDeleted);

        if (request.ActiveOnly == true)
            query = query.Where(f => f.IsActive);

        return await query
            .Select(f => new FleetListDto(
                f.Id,
                f.UserAccountId,
                f.UserAccount.FirstName,
                f.UserAccount.LastName,
                f.UserAccount.PhoneNumber,
                f.FleetTypeId,
                f.FleetType.Title,
                f.Plaque,
                f.StartDate,
                f.EndDate,
                f.IsActive))
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}

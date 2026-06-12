using CoreApplication.Application.Common.Exceptions;
using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Application.Features.Fleets.Common;
using CoreApplication.Domain.Entities.Fleet;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.Fleets.Queries.GetFleetById;

public class GetFleetByIdQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    : IRequestHandler<GetFleetByIdQuery, FleetDetailDto>
{
    public async Task<FleetDetailDto> Handle(GetFleetByIdQuery request, CancellationToken cancellationToken)
    {
        var courierId = currentUser.CourierId
            ?? throw new ForbiddenAccessException();

        return await context.Fleets
            .Where(f => f.Id == request.Id && f.CourierId == courierId && !f.IsDeleted)
            .Select(f => new FleetDetailDto(
                f.Id,
                f.UserAccountId,
                f.UserAccount.FirstName,
                f.UserAccount.LastName,
                f.UserAccount.PhoneNumber,
                f.CourierId,
                f.FleetTypeId,
                f.FleetType.Title,
                f.Plaque,
                f.DrivingLicense,
                f.StartDate,
                f.EndDate,
                f.Description,
                f.ProfileImage,
                f.InsuranceCode,
                f.IsActive))
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException(nameof(FleetEntity), request.Id);
    }
}

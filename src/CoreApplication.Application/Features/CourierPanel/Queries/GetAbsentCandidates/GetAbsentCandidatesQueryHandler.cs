using CoreApplication.Application.Common.Exceptions;
using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Application.Features.CourierPanel.Common;
using CoreApplication.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.CourierPanel.Queries.GetAbsentCandidates;

public class GetAbsentCandidatesQueryHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser,
    IDateTimeService dateTime)
    : IRequestHandler<GetAbsentCandidatesQuery, List<AbsentCandidateDto>>
{
    public async Task<List<AbsentCandidateDto>> Handle(
        GetAbsentCandidatesQuery request,
        CancellationToken cancellationToken)
    {
        var courierId = currentUser.CourierId ?? throw new ForbiddenAccessException();
        var today = dateTime.UtcNow.Date;

        return await context.ParcelCouriers
            .Where(pc =>
                pc.CourierId == courierId &&
                pc.Status == ParcelTrackingEnum.Processing &&
                pc.AssignedAt < today &&
                !pc.IsDeleted &&
                pc.Parcel.IsActive && !pc.Parcel.IsDeleted)
            .OrderBy(pc => pc.AssignedAt)
            .Select(pc => new AbsentCandidateDto(
                pc.Id,
                pc.ParcelId,
                pc.Parcel.Barcode,
                pc.LNumber,
                pc.ServiceType,
                pc.DistributeFirstName,
                pc.DistributeLastName,
                pc.DistributePhoneNumber,
                pc.DistributeAddress,
                pc.Parcel.HasCOD,
                pc.AssignedAt,
                pc.PromisedDeliveryAt,
                (int)(today - pc.AssignedAt.Date).TotalDays))
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}

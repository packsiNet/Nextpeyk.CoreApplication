using CoreApplication.Application.Common.Exceptions;
using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Application.Common.Models;
using CoreApplication.Application.Features.CourierPanel.Common;
using CoreApplication.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.CourierPanel.Queries.GetCourierParcels;

public class GetCourierParcelsQueryHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser)
    : IRequestHandler<GetCourierParcelsQuery, PaginatedList<CourierParcelItemDto>>
{
    public async Task<PaginatedList<CourierParcelItemDto>> Handle(
        GetCourierParcelsQuery request,
        CancellationToken cancellationToken)
    {
        var courierId = currentUser.CourierId ?? throw new ForbiddenAccessException();

        var query = context.ParcelCouriers
            .Where(pc => pc.CourierId == courierId && !pc.IsDeleted)
            .AsQueryable();

        if (request.Status.HasValue)
            query = query.Where(pc => pc.Status == request.Status.Value);

        if (request.From.HasValue)
            query = query.Where(pc => DateOnly.FromDateTime(pc.AssignedAt) >= request.From.Value);

        if (request.To.HasValue)
            query = query.Where(pc => DateOnly.FromDateTime(pc.AssignedAt) <= request.To.Value);

        if (request.FleetId.HasValue)
            query = query.Where(pc => pc.ParcelCourierFleets
                .Any(f => f.FleetId == request.FleetId.Value && !f.IsDeleted));

        var projected = query
            .OrderByDescending(pc => pc.AssignedAt)
            .Select(pc => new CourierParcelItemDto(
                pc.Id,
                pc.ParcelId,
                pc.Parcel.Barcode,
                pc.Status,
                pc.DistributeFirstName,
                pc.DistributeLastName,
                pc.DistributePhoneNumber,
                pc.DistributeAddress,
                pc.AssignedAt,
                pc.PromisedDeliveryAt,
                pc.Parcel.HasCOD,
                pc.Parcel.HasFMCG,
                pc.ParcelCourierFleets
                    .Where(f => !f.IsDeleted && f.Status != ParcelTrackingEnum.AssignCancelled)
                    .OrderByDescending(f => f.CreatedAt)
                    .Select(f => (int?)f.FleetId)
                    .FirstOrDefault(),
                pc.ParcelCourierFleets
                    .Where(f => !f.IsDeleted && f.Status != ParcelTrackingEnum.AssignCancelled)
                    .OrderByDescending(f => f.CreatedAt)
                    .Select(f => f.Fleet.UserAccount.FirstName + " " + f.Fleet.UserAccount.LastName)
                    .FirstOrDefault(),
                pc.ParcelCourierFleets
                    .Where(f => f.Status == ParcelTrackingEnum.FinalizedDelivery && !f.IsDeleted)
                    .Select(f => (DateTime?)f.DeliveredAt)
                    .FirstOrDefault(),
                pc.ParcelCourierFleets
                    .Where(f => f.Status == ParcelTrackingEnum.Returned && !f.IsDeleted)
                    .Select(f => f.ReturnReason)
                    .FirstOrDefault()));

        return await PaginatedList<CourierParcelItemDto>.CreateAsync(
            projected, request.PageNumber, request.PageSize, cancellationToken);
    }
}

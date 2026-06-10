using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Application.Common.Models;
using CoreApplication.Application.Features.Parcels.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Application.Features.Parcels.Queries.GetParcels;

public class GetParcelsQueryHandler(IApplicationDbContext context, ISenderContext senderContext)
    : IRequestHandler<GetParcelsQuery, PaginatedList<ParcelListDto>>
{
    public async Task<PaginatedList<ParcelListDto>> Handle(GetParcelsQuery request, CancellationToken cancellationToken)
    {
        var query = context.Parcels
            .Where(p => p.SenderId == senderContext.SenderId && !p.IsDeleted)
            .AsNoTracking();

        if (request.State.HasValue)
            query = query.Where(p => p.State == request.State.Value);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            query = query.Where(p =>
                p.Barcode.Contains(request.SearchTerm) ||
                p.ReceiverFirstName.Contains(request.SearchTerm) ||
                p.ReceiverLastName.Contains(request.SearchTerm) ||
                p.ReceiverPhoneNumber.Contains(request.SearchTerm));

        var projected = query
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => new ParcelListDto(
                p.Id, p.Barcode, p.State, p.ParcelType,
                p.ReceiverFirstName, p.ReceiverLastName,
                p.ReceiverPhoneNumber, p.ReceiverAddress,
                p.Weight, p.HasCOD, p.CreatedAt));

        return await PaginatedList<ParcelListDto>.CreateAsync(projected, request.PageNumber, request.PageSize, cancellationToken);
    }
}

using CoreApplication.Application.Common.Models;
using CoreApplication.Application.Features.Parcels.Common;
using CoreApplication.Domain.Enums;
using MediatR;

namespace CoreApplication.Application.Features.Parcels.Queries.GetParcels;

public record GetParcelsQuery(
    int PageNumber = 1,
    int PageSize = 20,
    ParcelState? State = null,
    string? SearchTerm = null) : IRequest<PaginatedList<ParcelListDto>>;

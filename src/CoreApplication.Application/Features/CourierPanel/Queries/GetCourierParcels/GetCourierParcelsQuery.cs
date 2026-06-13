using CoreApplication.Application.Common.Models;
using CoreApplication.Application.Features.CourierPanel.Common;
using CoreApplication.Domain.Enums;
using MediatR;

namespace CoreApplication.Application.Features.CourierPanel.Queries.GetCourierParcels;

public record GetCourierParcelsQuery(
    ParcelTrackingEnum? Status = null,
    int? FleetId = null,
    DateOnly? From = null,
    DateOnly? To = null,
    int PageNumber = 1,
    int PageSize = 20) : IRequest<PaginatedList<CourierParcelItemDto>>;

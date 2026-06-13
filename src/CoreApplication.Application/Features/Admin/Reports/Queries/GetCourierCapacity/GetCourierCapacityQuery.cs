using CoreApplication.Application.Features.Admin.Reports.Common;
using MediatR;

namespace CoreApplication.Application.Features.Admin.Reports.Queries.GetCourierCapacity;

public record GetCourierCapacityQuery(
    DateOnly? From = null,
    DateOnly? To = null) : IRequest<List<CourierCapacityDto>>;

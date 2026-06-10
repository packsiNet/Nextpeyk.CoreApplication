using CoreApplication.Application.Features.Couriers.Common;
using MediatR;

namespace CoreApplication.Application.Features.Couriers.Queries.GetCourierById;

public record GetCourierByIdQuery(int Id) : IRequest<CourierDetailDto>;

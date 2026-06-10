using CoreApplication.Application.Features.Couriers.Common;
using MediatR;

namespace CoreApplication.Application.Features.Couriers.Queries.GetBoxSizes;

public record GetBoxSizesQuery : IRequest<List<BoxSizeDto>>;

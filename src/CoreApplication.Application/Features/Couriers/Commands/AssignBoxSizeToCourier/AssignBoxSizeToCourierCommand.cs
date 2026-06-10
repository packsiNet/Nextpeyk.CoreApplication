using MediatR;

namespace CoreApplication.Application.Features.Couriers.Commands.AssignBoxSizeToCourier;

public record AssignBoxSizeToCourierCommand(
    int CourierId,
    int BoxSizeId,
    decimal PackagingPrice) : IRequest<int>;

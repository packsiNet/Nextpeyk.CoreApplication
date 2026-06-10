using MediatR;

namespace CoreApplication.Application.Features.Couriers.Commands.RemoveBoxSizeFromCourier;

public record RemoveBoxSizeFromCourierCommand(int CourierBoxSizeId) : IRequest;

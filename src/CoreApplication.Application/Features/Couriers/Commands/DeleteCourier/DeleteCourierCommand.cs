using MediatR;

namespace CoreApplication.Application.Features.Couriers.Commands.DeleteCourier;

public record DeleteCourierCommand(int Id) : IRequest;

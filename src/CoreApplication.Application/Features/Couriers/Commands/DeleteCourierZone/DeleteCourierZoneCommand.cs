using MediatR;

namespace CoreApplication.Application.Features.Couriers.Commands.DeleteCourierZone;

public record DeleteCourierZoneCommand(int Id) : IRequest;

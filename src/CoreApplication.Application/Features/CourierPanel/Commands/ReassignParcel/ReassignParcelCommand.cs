using MediatR;

namespace CoreApplication.Application.Features.CourierPanel.Commands.ReassignParcel;

public record ReassignParcelCommand(int ParcelCourierFleetId) : IRequest;

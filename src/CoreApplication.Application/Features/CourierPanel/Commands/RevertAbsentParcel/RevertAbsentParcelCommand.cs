using MediatR;

namespace CoreApplication.Application.Features.CourierPanel.Commands.RevertAbsentParcel;

public record RevertAbsentParcelCommand(int ParcelCourierId) : IRequest;

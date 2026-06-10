using MediatR;

namespace CoreApplication.Application.Features.Parcels.Commands.ConfirmParcel;

public record ConfirmParcelCommand(int ParcelId, int CourierId) : IRequest;

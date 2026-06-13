using MediatR;

namespace CoreApplication.Application.Features.CourierPanel.Commands.ConfirmAbsentParcels;

public record ConfirmAbsentParcelsCommand(List<int> ParcelCourierIds) : IRequest<int>;

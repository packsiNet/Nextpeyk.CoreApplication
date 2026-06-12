using CoreApplication.Application.Features.CourierPanel.Common;
using MediatR;

namespace CoreApplication.Application.Features.CourierPanel.Queries.GetProcessingParcels;

public record GetProcessingParcelsQuery : IRequest<List<ProcessingParcelDto>>;

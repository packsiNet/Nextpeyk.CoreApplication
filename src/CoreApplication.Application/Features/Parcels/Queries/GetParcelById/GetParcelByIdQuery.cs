using CoreApplication.Application.Features.Parcels.Common;
using MediatR;

namespace CoreApplication.Application.Features.Parcels.Queries.GetParcelById;

public record GetParcelByIdQuery(int Id) : IRequest<ParcelDetailDto>;

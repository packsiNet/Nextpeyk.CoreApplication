using CoreApplication.Application.Features.Parcels.Common;
using MediatR;

namespace CoreApplication.Application.Features.Parcels.Queries.InquireParcel;

public record InquireParcelQuery(
    double ReceiverLongitude,
    double ReceiverLatitude,
    decimal Weight,
    bool HasCOD,
    bool HasFMCG,
    bool HasFreightCollect) : IRequest<List<CourierOptionDto>>;

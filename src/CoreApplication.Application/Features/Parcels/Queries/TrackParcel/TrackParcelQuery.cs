using CoreApplication.Application.Features.Parcels.Common;
using MediatR;

namespace CoreApplication.Application.Features.Parcels.Queries.TrackParcel;

public record TrackParcelQuery(string Barcode) : IRequest<ParcelTrackDto>;

using CoreApplication.Application.Features.FleetApp.Common;
using MediatR;

namespace CoreApplication.Application.Features.FleetApp.Queries.GetAssignedParcels;

public record GetAssignedParcelsQuery : IRequest<List<AssignedParcelDto>>;

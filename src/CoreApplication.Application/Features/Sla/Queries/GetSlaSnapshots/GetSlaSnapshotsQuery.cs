using CoreApplication.Application.Common.Models;
using CoreApplication.Application.Features.Sla.Common;
using CoreApplication.Domain.Enums;
using MediatR;

namespace CoreApplication.Application.Features.Sla.Queries.GetSlaSnapshots;

public record GetSlaSnapshotsQuery(
    int PageNumber = 1,
    int PageSize = 20,
    int? CourierId = null,
    SlaPeriodType? PeriodType = null,
    DateOnly? From = null,
    DateOnly? To = null) : IRequest<PaginatedList<SlaSnapshotDto>>;

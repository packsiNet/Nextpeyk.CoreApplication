using CoreApplication.Application.Features.CourierPanel.Common;
using MediatR;

namespace CoreApplication.Application.Features.CourierPanel.Queries.GetAbsentCandidates;

public record GetAbsentCandidatesQuery : IRequest<List<AbsentCandidateDto>>;

using CoreApplication.Application.Features.Sla.Common;
using MediatR;

namespace CoreApplication.Application.Features.Sla.Queries.GetSlaGlobalConfig;

public record GetSlaGlobalConfigQuery : IRequest<SlaGlobalConfigDto?>;

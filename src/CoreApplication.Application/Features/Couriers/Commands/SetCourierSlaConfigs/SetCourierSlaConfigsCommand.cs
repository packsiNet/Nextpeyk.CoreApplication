using CoreApplication.Domain.Enums;
using MediatR;

namespace CoreApplication.Application.Features.Couriers.Commands.SetCourierSlaConfigs;

public record SlaConfigItem(
    ServiceType ServiceType,
    int SlaHours,
    decimal SuccessRateMin,
    decimal OnTimeMin,
    decimal ReturnRateMax);

public record SetCourierSlaConfigsCommand(
    int CourierId,
    List<SlaConfigItem> SlaConfigs) : IRequest;

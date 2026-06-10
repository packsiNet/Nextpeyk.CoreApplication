using CoreApplication.Domain.Enums;

namespace CoreApplication.Application.Features.Couriers.Common;

public record CourierSlaConfigDto(
    int Id,
    ServiceType ServiceType,
    int SlaHours,
    decimal SuccessRateMin,
    decimal OnTimeMin,
    decimal ReturnRateMax);

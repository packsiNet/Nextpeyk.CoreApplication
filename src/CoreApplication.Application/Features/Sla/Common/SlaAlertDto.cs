using CoreApplication.Domain.Enums;

namespace CoreApplication.Application.Features.Sla.Common;

public record SlaAlertDto(
    int Id,
    int CourierId,
    string CourierTitle,
    int SnapshotId,
    SlaPeriodType SnapshotPeriodType,
    DateOnly SnapshotPeriodStart,
    SlaAlertType AlertType,
    bool IsRead,
    DateTime CreatedAt);

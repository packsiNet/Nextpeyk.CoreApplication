using CoreApplication.Domain.Enums;

namespace CoreApplication.Application.Features.Admin.Reports.Common;

public record ActiveAlertSummaryDto(
    int CourierId,
    string CourierTitle,
    int AlertCount,
    List<SlaAlertType> AlertTypes);

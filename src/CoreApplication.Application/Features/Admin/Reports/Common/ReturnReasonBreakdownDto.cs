using CoreApplication.Domain.Enums;

namespace CoreApplication.Application.Features.Admin.Reports.Common;

public record ReturnReasonBreakdownDto(
    ReturnParcelReason Reason,
    int Count,
    decimal Percent);

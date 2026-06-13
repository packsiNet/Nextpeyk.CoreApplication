namespace CoreApplication.Application.Features.Senders.Common;

public record SenderListDto(
    int Id,
    string Title,
    string? ContractNumber,
    DateOnly? ContractStartDate,
    DateOnly? ContractEndDate,
    string? PhoneNumber,
    string? Email,
    bool IsActive);

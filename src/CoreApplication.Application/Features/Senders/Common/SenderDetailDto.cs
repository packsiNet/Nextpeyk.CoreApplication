namespace CoreApplication.Application.Features.Senders.Common;

public record SenderDetailDto(
    int Id,
    string Title,
    string? NationalCode,
    string? EconomicCode,
    string? RegistrationNumber,
    string? ContractNumber,
    DateOnly? ContractStartDate,
    DateOnly? ContractEndDate,
    string? Address,
    string? PhoneNumber,
    string? Email,
    bool IsActive,
    List<SenderCredentialDto> Credentials);

public record SenderCredentialDto(
    int Id,
    string ApiKey,
    string? Description,
    DateTime? ExpiresAt,
    bool IsActive);

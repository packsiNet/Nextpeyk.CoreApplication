using MediatR;

namespace CoreApplication.Application.Features.Senders.Commands.CreateSender;

public record CreateSenderCommand(
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
    string UserName,
    string Password,
    string? UserPhoneNumber) : IRequest<int>;

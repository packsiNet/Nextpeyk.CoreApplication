using MediatR;

namespace CoreApplication.Application.Features.Senders.Commands.UpdateSender;

public record UpdateSenderCommand(
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
    string? Email) : IRequest;

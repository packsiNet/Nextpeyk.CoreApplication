using CoreApplication.Application.Features.Senders.Commands.CreateSender;
using CoreApplication.Application.Features.Senders.Commands.DeleteSender;
using CoreApplication.Application.Features.Senders.Commands.GenerateApiCredential;
using CoreApplication.Application.Features.Senders.Commands.RevokeApiCredential;
using CoreApplication.Application.Features.Senders.Commands.UpdateSender;
using CoreApplication.Application.Features.Senders.Queries.GetSenderById;
using CoreApplication.Application.Features.Senders.Queries.GetSenders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreApplication.API.Controllers;

[Authorize(Roles = "Admin")]
public class SenderController : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetSendersQuery query, CancellationToken ct)
        => Ok(await Mediator.Send(query, ct));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
        => Ok(await Mediator.Send(new GetSenderByIdQuery(id), ct));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSenderCommand command, CancellationToken ct)
    {
        var senderId = await Mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = senderId }, new { senderId });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateSenderRequest request, CancellationToken ct)
    {
        await Mediator.Send(new UpdateSenderCommand(
            id, request.Title, request.NationalCode, request.EconomicCode,
            request.RegistrationNumber, request.ContractNumber,
            request.ContractStartDate, request.ContractEndDate,
            request.Address, request.PhoneNumber, request.Email), ct);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await Mediator.Send(new DeleteSenderCommand(id), ct);
        return NoContent();
    }

    // --- Credentials ---

    [HttpPost("{senderId:int}/credentials")]
    public async Task<IActionResult> GenerateCredential(
        int senderId,
        [FromBody] GenerateCredentialRequest request,
        CancellationToken ct)
    {
        var command = new GenerateApiCredentialCommand(senderId, request.Description, request.ExpiresAt);
        var result = await Mediator.Send(command, ct);

        // Secret returned once — client must save it
        return Ok(result);
    }

    [HttpDelete("credentials/{credentialId:int}")]
    public async Task<IActionResult> RevokeCredential(int credentialId, CancellationToken ct)
    {
        await Mediator.Send(new RevokeApiCredentialCommand(credentialId), ct);
        return NoContent();
    }
}

public record UpdateSenderRequest(
    string Title,
    string? NationalCode,
    string? EconomicCode,
    string? RegistrationNumber,
    string? ContractNumber,
    DateOnly? ContractStartDate,
    DateOnly? ContractEndDate,
    string? Address,
    string? PhoneNumber,
    string? Email);

public record GenerateCredentialRequest(string? Description, DateTime? ExpiresAt);

using CoreApplication.Application.Features.Senders.Commands.CreateSender;
using CoreApplication.Application.Features.Senders.Commands.GenerateApiCredential;
using CoreApplication.Application.Features.Senders.Commands.RevokeApiCredential;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreApplication.API.Controllers;

[Authorize(Roles = "Admin")]
public class SenderController : ApiControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSenderCommand command, CancellationToken ct)
    {
        var senderId = await Mediator.Send(command, ct);
        return CreatedAtAction(nameof(Create), new { id = senderId }, new { senderId });
    }

    [HttpPost("{senderId}/credentials")]
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

    [HttpDelete("credentials/{credentialId}")]
    public async Task<IActionResult> RevokeCredential(int credentialId, CancellationToken ct)
    {
        await Mediator.Send(new RevokeApiCredentialCommand(credentialId), ct);
        return NoContent();
    }
}

public record GenerateCredentialRequest(string? Description, DateTime? ExpiresAt);

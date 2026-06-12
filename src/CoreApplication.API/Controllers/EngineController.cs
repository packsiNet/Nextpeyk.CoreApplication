using CoreApplication.Application.Features.Engine.Commands.Confirm;
using CoreApplication.Application.Features.Engine.Commands.Inquiry;
using CoreApplication.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreApplication.API.Controllers;

[Authorize(AuthenticationSchemes = ApiKeyDefaults.AuthenticationScheme)]
public class EngineController : ApiControllerBase
{
    [HttpPost("inquiry")]
    public async Task<IActionResult> Inquiry([FromBody] InquiryCommand command, CancellationToken ct)
        => Ok(await Mediator.Send(command, ct));

    [HttpPost("confirm")]
    public async Task<IActionResult> Confirm([FromBody] ConfirmShipmentsCommand command, CancellationToken ct)
        => Ok(await Mediator.Send(command, ct));
}

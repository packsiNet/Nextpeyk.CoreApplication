using CoreApplication.Application.Features.Auth.Commands.Login;
using CoreApplication.Application.Features.Auth.Commands.SendOtp;
using CoreApplication.Application.Features.Auth.Commands.VerifyOtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreApplication.API.Controllers;

[AllowAnonymous]
public class AuthController : ApiControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken ct)
    {
        var result = await Mediator.Send(command, ct);
        return Ok(result);
    }

    [HttpPost("otp/send")]
    public async Task<IActionResult> SendOtp([FromBody] SendOtpCommand command, CancellationToken ct)
    {
        await Mediator.Send(command, ct);
        return Ok(new { message = "OTP sent successfully." });
    }

    [HttpPost("otp/verify")]
    public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpCommand command, CancellationToken ct)
    {
        var result = await Mediator.Send(command, ct);
        return Ok(result);
    }
}

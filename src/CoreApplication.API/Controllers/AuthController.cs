using CoreApplication.Application.Features.Auth.Commands.ChangePassword;
using CoreApplication.Application.Features.Auth.Commands.Login;
using CoreApplication.Application.Features.Auth.Commands.SendOtp;
using CoreApplication.Application.Features.Auth.Commands.VerifyOtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreApplication.API.Controllers;

public class AuthController : ApiControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken ct)
        => Ok(await Mediator.Send(command, ct));

    [AllowAnonymous]
    [HttpPost("otp/send")]
    public async Task<IActionResult> SendOtp([FromBody] SendOtpCommand command, CancellationToken ct)
    {
        await Mediator.Send(command, ct);
        return Ok(new { message = "OTP sent successfully." });
    }

    [AllowAnonymous]
    [HttpPost("otp/verify")]
    public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpCommand command, CancellationToken ct)
        => Ok(await Mediator.Send(command, ct));

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command, CancellationToken ct)
    {
        await Mediator.Send(command, ct);
        return NoContent();
    }
}

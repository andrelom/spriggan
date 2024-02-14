using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spriggan.Core;
using Spriggan.Core.Transport;
using Spriggan.Data.Identity.Contracts.Values;
using Spriggan.Foundation.Identity.Attributes;
using Spriggan.Module.Identity.Contracts;
using Spriggan.Module.Identity.Contracts.Features.ResetPassword;
using Spriggan.Module.Identity.Contracts.Features.SendResetPasswordToken;
using Spriggan.Module.Identity.Contracts.Features.SignIn;
using Spriggan.Module.Identity.Contracts.Features.SignUp;
using ControllerBase = Spriggan.Core.Web.ControllerBase;

namespace Spriggan.Module.Identity.Controllers;

[Tags("Identity: Account")]
public class AccountController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("signin")]
    [ProducesResponseType(typeof(Result<SignInResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SignIn([FromBody] SignInRequest request)
    {
        var response = await _mediator.Send(request);

        if (response.IsError(Errors.Validation))
        {
            return StatusCode(StatusCodes.Status400BadRequest, response);
        }

        return response.Ok
            ? Ok(response)
            : StatusCode(StatusCodes.Status401Unauthorized, response);
    }

    [UserRoles(Roles.Administrator.Name)]
    [HttpPost]
    [Route("signup")]
    [ProducesResponseType(typeof(Result<SignUpResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SignUp([FromBody] SignUpRequest request)
    {
        var response = await _mediator.Send(request);

        return response.Ok
            ? Ok(response)
            : StatusCode(StatusCodes.Status400BadRequest, response);
    }

    [UserRoles(Roles.Administrator.Name, Roles.User.Name)]
    [HttpPost]
    [Route("forgot-password")]
    [ProducesResponseType(typeof(Result<SendResetPasswordTokenResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ForgotPassword([FromBody] SendResetPasswordTokenRequest request)
    {
        var response = await _mediator.Send(request);

        return response.Ok
            ? Ok(response)
            : StatusCode(StatusCodes.Status400BadRequest, response);
    }

    [UserRoles(Roles.Administrator.Name, Roles.User.Name)]
    [HttpPost]
    [Route("reset-password")]
    [ProducesResponseType(typeof(Result<ResetPasswordResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var response = await _mediator.Send(request);

        if (response.Ok)
        {
            return Ok(response);
        }

        return response.Error.Equals(IdentityErrors.TokenExpired)
            ? StatusCode(StatusCodes.Status403Forbidden, response)
            : StatusCode(StatusCodes.Status400BadRequest, response);
    }
}

using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spriggan.Core;
using Spriggan.Data.Identity.Contracts.Values;
using Spriggan.Foundation.Identity.Attributes;
using Spriggan.Module.Identity.Contracts.Features.SendResetPasswordToken;

namespace Spriggan.Module.Identity.Features.SendResetPasswordToken;

[Tags("Accounts")]
[ApiVersion("1")]
[Route("v{version:apiVersion}/identity/forgot-password")]
public class SendResetPasswordTokenController : Core.Web.ControllerBase
{
    private readonly IMediator _mediator;

    public SendResetPasswordTokenController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [UserRoles(Roles.Administrator.Name, Roles.User.Name)]
    [HttpPost]
    [Route("")]
    [ProducesResponseType(typeof(Result<SendResetPasswordTokenResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ForgotPassword([FromBody] SendResetPasswordTokenRequest request)
    {
        var response = await _mediator.Send(request);

        return response.Ok
            ? Ok(response)
            : StatusCode(StatusCodes.Status400BadRequest, response);
    }
}
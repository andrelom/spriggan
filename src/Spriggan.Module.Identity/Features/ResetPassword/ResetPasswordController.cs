using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spriggan.Core;
using Spriggan.Data.Identity.Contracts.Values;
using Spriggan.Foundation.Identity.Attributes;
using Spriggan.Module.Identity.Contracts;
using Spriggan.Module.Identity.Contracts.Features.ResetPassword;
using ControllerBase = Spriggan.Core.Web.ControllerBase;

namespace Spriggan.Module.Identity.Features.ResetPassword;

[Tags("Accounts")]
[ApiVersion("1")]
[Route("v{version:apiVersion}/identity/reset-password")]
public class ResetPasswordController : ControllerBase
{
    private readonly IMediator _mediator;

    public ResetPasswordController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [UserRoles(Roles.Administrator.Name, Roles.User.Name)]
    [HttpPost]
    [Route("")]
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

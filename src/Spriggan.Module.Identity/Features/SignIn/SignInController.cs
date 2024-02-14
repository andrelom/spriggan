using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spriggan.Core;
using Spriggan.Module.Identity.Contracts.Features.SignIn;
using ControllerBase = Spriggan.Core.Web.ControllerBase;

namespace Spriggan.Module.Identity.Features.SignIn;

[ApiVersion("1"), Tags("Accounts")]
[Route("v{version:apiVersion}/identity/signin")]
public class SignInController : ControllerBase
{
    private readonly IMediator _mediator;

    public SignInController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("")]
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
}

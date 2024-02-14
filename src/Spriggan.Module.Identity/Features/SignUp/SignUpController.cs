using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spriggan.Core;
using Spriggan.Data.Identity.Contracts.Values;
using Spriggan.Foundation.Identity.Attributes;
using Spriggan.Module.Identity.Contracts.Features.SignUp;

namespace Spriggan.Module.Identity.Features.SignUp;

[Tags("Accounts")]
[ApiVersion("1")]
[Route("v{version:apiVersion}/identity/signup")]
public class SignUpController : Core.Web.ControllerBase
{
    private readonly IMediator _mediator;

    public SignUpController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [UserRoles(Roles.Administrator.Name)]
    [HttpPost]
    [Route("")]
    [ProducesResponseType(typeof(Result<SignUpResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SignUp([FromBody] SignUpRequest request)
    {
        var response = await _mediator.Send(request);

        return response.Ok
            ? Ok(response)
            : StatusCode(StatusCodes.Status400BadRequest, response);
    }
}

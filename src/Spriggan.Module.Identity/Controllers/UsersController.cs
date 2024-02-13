using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spriggan.Core;
using Spriggan.Core.Transport;
using Spriggan.Data.Identity.Contracts.Values;
using Spriggan.Foundation.Identity.Attributes;
using Spriggan.Module.Identity.Contracts.Features.GetAllUser;
using Spriggan.Module.Identity.Contracts.Features.GetUserByName;

namespace Spriggan.Module.Identity.Controllers;

[ApiVersion("1")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [UserRoles(Roles.Administrator.Name)]
    [HttpGet]
    [Route("")]
    [ProducesResponseType(typeof(Result<GetAllUserResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllUserRequest request)
    {
        var response = await _mediator.Send(request);

        return response.Ok
            ? Ok(response)
            : StatusCode(StatusCodes.Status400BadRequest, response);
    }

    [UserRoles(Roles.Administrator.Name)]
    [HttpGet]
    [Route("{username}")]
    [ProducesResponseType(typeof(Result<GetUserByNameResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByName([FromRoute] GetUserByNameRequest request)
    {
        var response = await _mediator.Send(request);

        return response.Ok
            ? Ok(response)
            : StatusCode(StatusCodes.Status400BadRequest, response);
    }
}

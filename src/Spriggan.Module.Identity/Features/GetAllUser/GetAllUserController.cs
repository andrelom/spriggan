using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spriggan.Core;
using Spriggan.Data.Identity.Contracts.Values;
using Spriggan.Foundation.Identity.Attributes;
using Spriggan.Module.Identity.Contracts.Features.GetAllUser;
using ControllerBase = Spriggan.Core.Web.ControllerBase;

namespace Spriggan.Module.Identity.Features.GetAllUser;

[Tags("Users")]
[Route("/identity/users")]
public class GetAllUserController : ControllerBase
{
    private readonly IMediator _mediator;

    public GetAllUserController(IMediator mediator)
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
}

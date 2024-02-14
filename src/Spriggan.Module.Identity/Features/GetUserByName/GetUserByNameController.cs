using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spriggan.Core;
using Spriggan.Data.Identity.Contracts.Values;
using Spriggan.Foundation.Identity.Attributes;
using Spriggan.Module.Identity.Contracts.Features.GetUserByName;
using ControllerBase = Spriggan.Core.Web.ControllerBase;

namespace Spriggan.Module.Identity.Features.GetUserByName;

[Tags("Identity: Users")]
[Route("/identity/users")]
public class GetUserByNameController : ControllerBase
{
    private readonly IMediator _mediator;

    public GetUserByNameController(IMediator mediator)
    {
        _mediator = mediator;
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

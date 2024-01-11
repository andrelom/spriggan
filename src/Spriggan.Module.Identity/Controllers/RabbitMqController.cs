using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spriggan.Core;
using Spriggan.Core.Transport;
using Spriggan.Module.Identity.Contracts.Features.SignIn;

namespace Spriggan.Module.Identity.Controllers;

[ApiVersion("1")]
public class RabbitMqController : ControllerApi
{
    private readonly IRabbitMqBus _bus;

    public RabbitMqController(IRabbitMqBus bus)
    {
        _bus = bus;
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("send")]
    [ProducesResponseType(typeof(Result<SignInResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SignIn([FromBody] SignInRequest request)
    {
        var response = await _bus.Send(request);

        if (response.IsError(Errors.Validation))
        {
            return StatusCode(StatusCodes.Status400BadRequest, response);
        }

        return response.Ok
            ? Ok(response)
            : StatusCode(StatusCodes.Status401Unauthorized, response);
    }
}

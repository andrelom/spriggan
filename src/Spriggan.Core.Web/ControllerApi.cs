using Microsoft.AspNetCore.Mvc;

namespace Spriggan.Core.Web;

[ApiController]
[Route("[controller]")]
public abstract class ControllerApi : ControllerBase
{
    public new OkObjectResult Ok()
    {
        return base.Ok(new { });
    }
}

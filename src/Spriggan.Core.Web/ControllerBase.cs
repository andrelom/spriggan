using Microsoft.AspNetCore.Mvc;

namespace Spriggan.Core.Web;

[ApiController]
public abstract class ControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public new OkObjectResult Ok()
    {
        return base.Ok(new { });
    }
}

using Microsoft.AspNetCore.Mvc;

namespace Spriggan.Module.Identity;

[Route("identity/[controller]")]
public abstract class ControllerApi : Core.Web.ControllerApi
{
    // Intentionally left empty.
}

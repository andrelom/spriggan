using Microsoft.AspNetCore.Mvc;

namespace Spriggan.Module.Identity;

[Route("v{version:apiVersion}/identity/[controller]")]
public abstract class ControllerBase : Core.Web.ControllerBase
{
    // Intentionally left empty.
}

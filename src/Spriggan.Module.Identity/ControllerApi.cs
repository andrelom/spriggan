using Microsoft.AspNetCore.Mvc;

namespace Spriggan.Module.Identity;

[Route("v{version:apiVersion}/identity/[controller]")]
public abstract class ControllerApi : Core.Web.ControllerApi
{
    // Intentionally left empty.
}

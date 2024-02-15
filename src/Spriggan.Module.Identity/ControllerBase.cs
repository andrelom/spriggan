using Microsoft.AspNetCore.Mvc;

namespace Spriggan.Module.Identity;

[Route("/identity/[controller]")]
public abstract class ControllerBase : Core.Web.ControllerBase
{
    // Intentionally left empty.
}

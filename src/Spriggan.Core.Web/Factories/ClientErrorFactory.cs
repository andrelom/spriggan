using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Spriggan.Core.Web.Factories;

public class ClientErrorFactory : IClientErrorFactory
{
    public IActionResult GetClientError(ActionContext context, IClientErrorActionResult error)
    {
        var result = Result.Fail(Errors.Whoops, new
        {
            context.HttpContext.TraceIdentifier
        });

        return new ObjectResult(result)
        {
            StatusCode = error.StatusCode
        };
    }
}

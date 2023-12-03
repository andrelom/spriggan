using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.JsonWebTokens;
using Spriggan.Core.Extensions;
using Spriggan.Foundation.Identity.Attributes;

namespace Spriggan.Foundation.Identity.Filters;

public class UserIdAttributeActionFilter : IActionFilter
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserIdAttributeActionFilter(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var user = _httpContextAccessor.HttpContext?.User;

        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument == null)
            {
                continue;
            }

            var type = argument.GetType();
            var properties = type.GetProperties();

            foreach (var property in properties)
            {
                HandleUserIdAttribute(user, property, argument);
            }
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // Intentionally left empty.
    }

    #region Private Methods

    private static void HandleUserIdAttribute(ClaimsPrincipal? user, PropertyInfo property, object argument)
    {
        if (property.GetCustomAttribute<UserIdAttribute>() == null)
        {
            return;
        }

        if (!property.GetValue(argument)?.Equals(Guid.Empty) ?? false)
        {
            return;
        }

        if (user?.FindFirstValue(JwtRegisteredClaimNames.Sub)?.ToGuid() is not { } id)
        {
            return;
        }

        property.SetValue(argument, id, null);
    }

    #endregion
}

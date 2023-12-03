using System.Reflection;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Spriggan.Foundation.Identity.Attributes;

namespace Spriggan.Foundation.Identity.Filters;

public class UserRolesAttributeActionFilter : IActionFilter
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserRolesAttributeActionFilter(IHttpContextAccessor httpContextAccessor)
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
                HandleUserRolesAttribute(user, argument, property);
            }
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // Intentionally left empty.
    }

    #region Private Methods

    private static void HandleUserRolesAttribute(IPrincipal? user, object argument, PropertyInfo property)
    {
        if (property.GetCustomAttribute<UserRolesAttribute>() is not { } attribute)
        {
            return;
        }

        var roles = attribute.Roles?.Split(",");
        var authorized = roles?.Any(role => user?.IsInRole(role) ?? false) ?? false;

        if (!authorized)
        {
            property.SetValue(argument, default, null);
        }
    }

    #endregion
}

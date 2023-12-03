using Microsoft.AspNetCore.Authorization;

namespace Spriggan.Foundation.Identity.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property)]
public class UserRolesAttribute : AuthorizeAttribute
{
    public UserRolesAttribute(params string[] roles)
    {
        Roles = string.Join(",", roles);
    }
}

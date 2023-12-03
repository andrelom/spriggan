using Microsoft.AspNetCore.Identity;

namespace Spriggan.Foundation.Identity.Extensions;

public static class IdentityErrorExtensions
{
    public static string[] ToArray(this IEnumerable<IdentityError> errors)
    {
        return errors?.Select(error => error.Description).ToArray() ?? Array.Empty<string>();
    }
}

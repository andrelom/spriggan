using Microsoft.IdentityModel.Tokens;

namespace Spriggan.Module.Identity.Providers;

public interface ISecurityKeyProvider
{
    SecurityKey Generate();
}

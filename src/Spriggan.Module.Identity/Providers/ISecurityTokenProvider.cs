using Microsoft.IdentityModel.Tokens;
using Spriggan.Data.Identity.Contracts.Entities;

namespace Spriggan.Module.Identity.Providers;

public interface ISecurityTokenProvider
{
    Task<SecurityToken> Generate(User user);
}

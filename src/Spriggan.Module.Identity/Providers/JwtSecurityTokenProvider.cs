using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Spriggan.Core.Extensions;
using Spriggan.Data.Identity.Contracts.Entities;
using Spriggan.Foundation.Identity.Options;

namespace Spriggan.Module.Identity.Providers;

public class JwtSecurityTokenProvider : ISecurityTokenProvider
{
    private readonly JwtOptions _jwtOptions;

    private readonly UserManager<User> _userManager;

    private readonly ISecurityKeyProvider _securityKeyProvider;

    public JwtSecurityTokenProvider(
        IConfiguration configuration,
        UserManager<User> userManager,
        ISecurityKeyProvider securityKeyProvider)
    {
        _jwtOptions = configuration.Load<JwtOptions>();
        _userManager = userManager;
        _securityKeyProvider = securityKeyProvider;
    }

    public async Task<SecurityToken> Generate(User user)
    {
        var key = _securityKeyProvider.Generate();
        var roles = await _userManager.GetRolesAsync(user);

        return new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            expires: CreateJwtExpirationDate(roles),
            claims: CreateJwtUserClaims(user, roles),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha512));
    }

    #region Private Methods

    private DateTime CreateJwtExpirationDate(IEnumerable<string> roles)
    {
        return roles.Any(role => _jwtOptions.RolesWithLongTermExpiresIn.Contains(role, StringComparer.OrdinalIgnoreCase))
            ? DateTime.UtcNow.AddDays(_jwtOptions.LongTermExpiresIn)
            : DateTime.UtcNow.AddDays(_jwtOptions.ExpiresIn);
    }

    private static IEnumerable<Claim>? CreateJwtUserClaims(User user, IEnumerable<string> roles)
    {
        if (string.IsNullOrWhiteSpace(user.UserName))
        {
            return default;
        }

        // More at: https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
        var claims = new List<Claim>
        {
            // Subject - Identifier for the End-User at the Issuer.
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            // End-User's full name in displayable form including all name parts, possibly including titles and suffixes, ordered
            // according to the End-User's locale and preferences.
            new(JwtRegisteredClaimNames.Name, user.UserName),
            // A unique identifier for the token, which can be used to prevent reuse of the token.
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        return claims;
    }

    #endregion
}

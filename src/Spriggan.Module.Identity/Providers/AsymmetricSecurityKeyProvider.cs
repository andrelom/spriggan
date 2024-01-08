using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Spriggan.Core.Extensions;
using Spriggan.Foundation.Identity.Options;

namespace Spriggan.Module.Identity.Providers;

public class AsymmetricSecurityKeyProvider : ISecurityKeyProvider
{
    private readonly JwtOptions _jwtOptions;

    public AsymmetricSecurityKeyProvider(IConfiguration configuration)
    {
        _jwtOptions = configuration.Load<JwtOptions>();
    }

    public SecurityKey Generate()
    {
        using var rsa = RSA.Create();

        rsa.ImportSubjectPublicKeyInfo(
            Convert.FromBase64String(_jwtOptions.IssuerKey),
            out _
        );

        return new RsaSecurityKey(rsa);
    }
}

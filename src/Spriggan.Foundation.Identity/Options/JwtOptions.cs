using System.ComponentModel.DataAnnotations;
using Spriggan.Core.Attributes;

namespace Spriggan.Foundation.Identity.Options;

[Option("Foundation:Identity:JWT")]
public class JwtOptions
{
    [Required]
    public int ExpiresIn { get; set; }

    public int LongTermExpiresIn { get; set; } = 360;

    [Required]
    public string Audience { get; set; } = null!;

    [Required]
    public string Issuer { get; set; } = null!;

    [Required]
    [Environment("JWT_ISSUER_KEY")]
    public string IssuerKey { get; set; } = null!;

    public string[] RolesWithLongTermExpiresIn { get; set; } = Array.Empty<string>();
}

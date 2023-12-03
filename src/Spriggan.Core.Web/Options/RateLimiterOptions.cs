using System.ComponentModel.DataAnnotations;
using Spriggan.Core.Attributes;

namespace Spriggan.Core.Web.Options;

[Option("Core:Web:RateLimiter")]
public class RateLimiterOptions
{
    [Required]
    public int Window { get; set; } = 5;

    [Required]
    public int PermitLimit { get; set; } = 10;

    [Required]
    public int QueueLimit { get; set; } = 2;
}

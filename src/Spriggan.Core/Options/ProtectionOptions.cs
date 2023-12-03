using System.ComponentModel.DataAnnotations;
using Spriggan.Core.Attributes;

namespace Spriggan.Core.Options;

[Option("Core:Protection")]
public class ProtectionOptions
{
    [Required]
    public string Name { get; set; } = null!;

    [Required]
    [Environment("PROTECTION_KEYS_PATH")]
    public string Path { get; set; } = null!;
}

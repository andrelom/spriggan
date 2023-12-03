using System.ComponentModel.DataAnnotations;
using Spriggan.Core.Attributes;

namespace Spriggan.Core.Web.Options;

[Option("Core:Web:Culture")]
public class CultureOptions
{
    [Required]
    public string Default { get; set; } = "pt-BR";

    [Required]
    public string[] Supported { get; set; } = new[] { "pt-BR" };
}

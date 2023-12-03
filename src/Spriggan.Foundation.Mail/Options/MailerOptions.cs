using System.ComponentModel.DataAnnotations;
using Spriggan.Core.Attributes;

namespace Spriggan.Foundation.Mail.Options;

[Option("Foundation:Mail:Mailer")]
public class MailerOptions
{
    [Required]
    [Environment("SEND_GRID_API_KEY")]
    public string SendGridApiKey { get; set; } = null!;
}

namespace Spriggan.Foundation.Mail.Models;

public class SendEmailSettings
{
    public string SenderEmail { get; set; } = null!;

    public string SenderName { get; set; } = null!;

    public string RecipientEmail { get; set; } = null!;

    public string RecipientName { get; set; } = null!;

    public string Subject { get; set; } = null!;

    public string Content { get; set; } = null!;
}

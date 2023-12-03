using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using Spriggan.Core;
using Spriggan.Core.Extensions;
using Spriggan.Foundation.Mail.Models;
using Spriggan.Foundation.Mail.Options;

namespace Spriggan.Foundation.Mail;

internal class Mailer : IMailer
{
    private readonly ILogger<Mailer> _logger;

    private readonly SendGridClient _client;

    public Mailer(IConfiguration configuration, ILogger<Mailer> logger)
    {
        var options = configuration.Load<MailerOptions>();

        _logger = logger;
        _client = new SendGridClient(options.SendGridApiKey);
    }

    public async Task<Result> SendEmail(SendEmailSettings settings, CancellationToken cancel = default)
    {
        const string mimetype = "text/html";

        var message = new SendGridMessage();

        message.SetFrom(settings.SenderEmail, settings.SenderName);

        message.AddTo(settings.RecipientEmail, settings.RecipientName);

        message.SetSubject(settings.Subject);

        message.AddContent(mimetype, settings.Content);

        var response = await _client.SendEmailAsync(message, cancel);

        if (response.IsSuccessStatusCode)
        {
            return Result.Success();
        }

        var error = await response.DeserializeResponseBodyAsync();

        _logger.LogError("Mailer ({0})", error.ToJson());

        return Result.Fail(Errors.Whoops);
    }
}

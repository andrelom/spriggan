using Spriggan.Core;
using Spriggan.Foundation.Mail.Models;

namespace Spriggan.Foundation.Mail;

public interface IMailer
{
    Task<Result> SendEmail(SendEmailSettings settings, CancellationToken cancel = default);
}

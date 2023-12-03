using Spriggan.Core;
using Spriggan.Core.Transport;

namespace Spriggan.Module.Identity.Contracts.Features.SendResetPasswordToken;

public class SendResetPasswordTokenRequest : IRequest<Result<SendResetPasswordTokenResponse>>
{
    public string UserName { get; set; } = null!;
}

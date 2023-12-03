using Spriggan.Core;
using Spriggan.Core.Transport;

namespace Spriggan.Module.Identity.Contracts.Features.ResetPassword;

public class ResetPasswordRequest : IRequest<Result<ResetPasswordResponse>>
{
    public string Token { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;
}

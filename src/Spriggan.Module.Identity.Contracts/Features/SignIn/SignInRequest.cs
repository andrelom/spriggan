using Spriggan.Core;
using Spriggan.Core.Transport;

namespace Spriggan.Module.Identity.Contracts.Features.SignIn;

public class SignInRequest : IRequest<Result<SignInResponse>>
{
    public string? UserName { get; set; }

    public string? Password { get; set; }
}

using Spriggan.Core;
using Spriggan.Core.Transport;

namespace Spriggan.Module.Identity.Contracts.Features.SignUp;

public class SignUpRequest : IRequest<Result<SignUpResponse>>
{
    public string Email { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;
}

using Microsoft.AspNetCore.Identity;
using Spriggan.Core;
using Spriggan.Core.Transport;
using Spriggan.Data.Identity.Contracts.Entities;
using Spriggan.Module.Identity.Contracts;
using Spriggan.Module.Identity.Contracts.Features.SendResetPasswordToken;

namespace Spriggan.Module.Identity.Features.SendResetPasswordToken;

public class SendResetPasswordTokenConsumer : Consumer<SendResetPasswordTokenRequest, Result<SendResetPasswordTokenResponse>>
{
    private readonly UserManager<User> _userManager;

    public SendResetPasswordTokenConsumer(
        UserManager<User> userManager,
        IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _userManager = userManager;
    }

    protected override async Task<Result<SendResetPasswordTokenResponse>> Handle(SendResetPasswordTokenRequest request)
    {
        var user = await _userManager.FindByNameAsync(request.UserName);

        if (user == null)
        {
            return Result<SendResetPasswordTokenResponse>.Fail(IdentityErrors.UserNotFound);
        }

        // TODO: Enviar o token por e-mail.

        return Result<SendResetPasswordTokenResponse>.Success(new()
        {
            Token = await _userManager.GeneratePasswordResetTokenAsync(user)
        });
    }
}

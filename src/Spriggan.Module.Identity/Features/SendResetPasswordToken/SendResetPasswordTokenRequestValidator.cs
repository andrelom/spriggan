using FluentValidation;
using Spriggan.Module.Identity.Contracts.Features.SendResetPasswordToken;

namespace Spriggan.Module.Identity.Features.SendResetPasswordToken;

public class SendResetPasswordTokenRequestValidator : AbstractValidator<SendResetPasswordTokenRequest>
{
    public SendResetPasswordTokenRequestValidator()
    {
        RuleFor(model => model.UserName)
            .NotEmpty();
    }
}

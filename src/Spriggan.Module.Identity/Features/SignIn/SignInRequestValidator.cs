using FluentValidation;
using Spriggan.Module.Identity.Contracts.Features.SignIn;

namespace Spriggan.Module.Identity.Features.SignIn;

public class SignInRequestValidator : AbstractValidator<SignInRequest>
{
    public SignInRequestValidator()
    {
        RuleFor(model => model.UserName)
            .NotEmpty();

        RuleFor(model => model.Password)
            .NotEmpty();
    }
}

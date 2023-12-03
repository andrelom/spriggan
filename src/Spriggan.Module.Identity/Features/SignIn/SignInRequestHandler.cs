using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using Spriggan.Core;
using Spriggan.Core.Transport;
using Spriggan.Data.Identity.Contracts.Entities;
using Spriggan.Module.Identity.Contracts;
using Spriggan.Module.Identity.Contracts.Features.SignIn;
using Spriggan.Module.Identity.Providers;

namespace Spriggan.Module.Identity.Features.SignIn;

public class SignInRequestHandler : IRequestHandler<SignInRequest, Result<SignInResponse>>
{
    private readonly UserManager<User> _userManager;

    private readonly ISecurityTokenProvider _securityTokenProvider;

    public SignInRequestHandler(
        UserManager<User> userManager,
        ISecurityTokenProvider securityTokenProvider)
    {
        _userManager = userManager;
        _securityTokenProvider = securityTokenProvider;
    }

    public async Task<Result<SignInResponse>> Handle(SignInRequest request, CancellationToken cancel = default)
    {
        if (await AuthorizeUser(request.UserName, request.Password) is not { } user)
        {
            return Result<SignInResponse>.Fail(IdentityErrors.InvalidCredentials);
        }

        if (!await _userManager.IsEmailConfirmedAsync(user))
        {
            return Result<SignInResponse>.Fail(IdentityErrors.EmailNotConfirmed);
        }

        var jwt = await _securityTokenProvider.Generate(user);
        var handler = new JwtSecurityTokenHandler();

        return Result<SignInResponse>.Success(new()
        {
            Id = user.Id,
            AccessToken = handler.WriteToken(jwt),
            ValidTo = jwt.ValidTo
        });
    }

    #region Private Methods

    private async Task<User?> AuthorizeUser(string? username, string? password)
    {
        if (string.IsNullOrWhiteSpace(username) ||
            string.IsNullOrWhiteSpace(password))
        {
            return default;
        }

        if (await _userManager.FindByNameAsync(username) is not { } user)
        {
            return default;
        }

        if (!await _userManager.CheckPasswordAsync(user, password))
        {
            return default;
        }

        return user;
    }

    #endregion
}

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Spriggan.Core;
using Spriggan.Core.Transport;
using Spriggan.Data.Identity.Contracts.Entities;
using Spriggan.Data.Identity.Contracts.Values;
using Spriggan.Foundation.Identity.Extensions;
using Spriggan.Module.Identity.Contracts;
using Spriggan.Module.Identity.Contracts.Features.SignUp;

namespace Spriggan.Module.Identity.Features.SignUp;

public class SignUpConsumer : Consumer<SignUpRequest, Result<SignUpResponse>>
{
    private readonly ILogger<SignUpConsumer> _logger;

    private readonly UserManager<User> _userManager;

    public SignUpConsumer(
        ILogger<SignUpConsumer> logger,
        UserManager<User> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    protected override async Task<Result<SignUpResponse>> Handle(SignUpRequest request)
    {
        if (await IsUserAvailable(request) is { Ok: false } isUserAvailableResult)
        {
            return Result<SignUpResponse>.Fail(isUserAvailableResult);
        }

        if (await CreateUser(request) is { Ok: false } createUserResult)
        {
            return Result<SignUpResponse>.Fail(createUserResult);
        }

        var user = await _userManager.FindByNameAsync(request.UserName);

        if (user == null)
        {
            // TODO: Criar uma ação de compensação.
            return Result<SignUpResponse>.Fail(Errors.NotFound);
        }

        if (await AddUserToRole(user) is { Ok: false } addUserToRoleResult)
        {
            // TODO: Criar uma ação de compensação.
            return Result<SignUpResponse>.Fail(addUserToRoleResult);
        }

        return Result<SignUpResponse>.Success(new()
        {
            Id = user.Id
        });
    }

    #region Private Methods

    private async Task<Result> IsUserAvailable(SignUpRequest request)
    {
        if (await _userManager.FindByEmailAsync(request.Email) != null)
        {
            return Result.Fail(IdentityErrors.EmailAlreadyInUse);
        }

        if (await _userManager.FindByNameAsync(request.UserName) != null)
        {
            return Result.Fail(IdentityErrors.UserNameAlreadyInUse);
        }

        return Result.Success();
    }

    private async Task<Result> CreateUser(SignUpRequest request)
    {
        var user = new User
        {
            Email = request.Email,
            UserName = request.UserName,
            SecurityStamp = Guid.NewGuid().ToString(),
            EmailConfirmed = true
        };

        if (await _userManager.CreateAsync(user, request.Password) is { Succeeded: false } result)
        {
            return Result.Fail(Errors.Validation, new Dictionary<string, object>
            {
                { "Validations", result.Errors.ToArray() },
            });
        }

        return Result.Success();
    }

    private async Task<Result> AddUserToRole(User? user)
    {
        if (user != null && await _userManager.AddToRoleAsync(user, Roles.User.Name) is { Succeeded: true })
        {
            return Result.Success();
        }

        _logger.LogError("Could not assign Role to User");

        return Result.Fail(Errors.Whoops);
    }

    #endregion
}

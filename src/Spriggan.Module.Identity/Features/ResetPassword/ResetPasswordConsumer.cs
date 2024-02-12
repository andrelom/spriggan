﻿using Microsoft.AspNetCore.Identity;
using Spriggan.Core;
using Spriggan.Core.Transport;
using Spriggan.Data.Identity.Contracts.Entities;
using Spriggan.Module.Identity.Contracts;
using Spriggan.Module.Identity.Contracts.Features.ResetPassword;

namespace Spriggan.Module.Identity.Features.ResetPassword;

public class ResetPasswordConsumer : Consumer<ResetPasswordRequest, Result<ResetPasswordResponse>>
{
    private readonly UserManager<User> _userManager;

    public ResetPasswordConsumer(
        UserManager<User> userManager,
        IServiceProvider services) : base(services)
    {
        _userManager = userManager;
    }

    protected override async Task<Result<ResetPasswordResponse>> Handle(ResetPasswordRequest request)
    {
        var user = await _userManager.FindByNameAsync(request.UserName);

        if (user == null)
        {
            return Result<ResetPasswordResponse>.Fail(IdentityErrors.UserNotFound);
        }

        var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);

        return result.Succeeded
            ? Result<ResetPasswordResponse>.Success(new())
            : Result<ResetPasswordResponse>.Fail(IdentityErrors.TokenExpired);
    }
}

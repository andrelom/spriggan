﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Spriggan.Core;
using Spriggan.Core.Transport;
using Spriggan.Data.Identity.Contracts.Entities;
using Spriggan.Module.Identity.Contracts;
using Spriggan.Module.Identity.Contracts.Features.GetUserByName;

namespace Spriggan.Module.Identity.Features.GetUserByName;

public class GetUserByNameConsumer : Consumer<GetUserByNameRequest, Result<GetUserByNameResponse>>
{
    private readonly IMapper _mapper;

    private readonly UserManager<User> _userManager;

    public GetUserByNameConsumer(
        IMapper mapper,
        UserManager<User> userManager,
        IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _mapper = mapper;
        _userManager = userManager;
    }

    protected override async Task<Result<GetUserByNameResponse>> Handle(GetUserByNameRequest request)
    {
        var user = await _userManager.FindByNameAsync(request.UserName);

        if (user == null)
        {
            return Result<GetUserByNameResponse>.Fail(IdentityErrors.UserNotFound);
        }

        var response = _mapper.Map<GetUserByNameResponse>(user);

        return Result<GetUserByNameResponse>.Success(response);
    }
}

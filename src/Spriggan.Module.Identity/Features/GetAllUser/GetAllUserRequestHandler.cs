﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Spriggan.Core;
using Spriggan.Core.Extensions;
using Spriggan.Core.Transport;
using Spriggan.Data.Identity.Contracts.Entities;
using Spriggan.Module.Identity.Contracts.Features.GetAllUser;

namespace Spriggan.Module.Identity.Features.GetAllUser;

public class GetAllUserRequestHandler : IRequestHandler<GetAllUserRequest, Result<GetAllUserResponse>>
{
    private readonly IMapper _mapper;

    private readonly UserManager<User> _userManager;

    public GetAllUserRequestHandler(
        IMapper mapper,
        UserManager<User> userManager)
    {
        _mapper = mapper;
        _userManager = userManager;
    }

    public Task<Result<GetAllUserResponse>> Handle(GetAllUserRequest request, CancellationToken cancel = default)
    {
        var users = _userManager.Users
            .OrderBy(user => user.NormalizedUserName)
            .Paginate(request.PageNumber, request.PageSize);

        return Task.FromResult(Result<GetAllUserResponse>.Success(new()
        {
            Users = _mapper.Map<IEnumerable<Contracts.Models.User>>(users)
        }));
    }
}

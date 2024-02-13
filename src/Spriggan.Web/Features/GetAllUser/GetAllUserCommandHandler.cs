using FastEndpoints;
using Spriggan.Core;

namespace Spriggan.Web.Features.GetAllUser;

public class GetAllUserCommandHandler : ICommandHandler<GetAllUserRequest, IResult<GetAllUserResponse>>
{
    public Task<IResult<GetAllUserResponse>> ExecuteAsync(GetAllUserRequest command, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}

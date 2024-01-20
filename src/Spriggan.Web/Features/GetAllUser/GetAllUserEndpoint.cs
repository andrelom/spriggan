using FastEndpoints;
using Spriggan.Core;

namespace Spriggan.Web.Features.GetAllUser;

public class GetAllUserEndpoint : Endpoint<GetAllUserRequest, Result<GetAllUserResponse>>
{
    public override void Configure()
    {
        AllowAnonymous();

        Get("/api/user");
    }

    public override async Task HandleAsync(GetAllUserRequest request, CancellationToken token)
    {
        var response = new Result<GetAllUserResponse>();

        await SendAsync(response, cancellation: token);
    }
}

using FastEndpoints;
using Spriggan.Core;

namespace Spriggan.Web.Features.GetAllUser;

public class GetAllUserEndpoint : Endpoint<GetAllUserRequest, IResult<GetAllUserResponse>>
{
    public override void Configure()
    {
        AllowAnonymous();

        Get("/api/user");
    }

    public override async Task HandleAsync(GetAllUserRequest request, CancellationToken token)
    {
        var response = await request.ExecuteAsync(token);

        await SendAsync(response, cancellation: token);
    }
}

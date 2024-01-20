using FastEndpoints;

namespace Spriggan.Web.Features.GetAllUser;

public class GetAllUserEndpoint : Endpoint<GetAllUserRequest, GetAllUserResponse>
{
    public override void Configure()
    {
        AllowAnonymous();

        Get("/api/user");
    }

    public override async Task HandleAsync(GetAllUserRequest request, CancellationToken token)
    {
        var response = new GetAllUserResponse();

        await SendAsync(response, cancellation: token);
    }
}

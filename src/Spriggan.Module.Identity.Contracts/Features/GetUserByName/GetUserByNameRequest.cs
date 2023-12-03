using Spriggan.Core;
using Spriggan.Core.Transport;

namespace Spriggan.Module.Identity.Contracts.Features.GetUserByName;

public class GetUserByNameRequest : IRequest<Result<GetUserByNameResponse>>
{
    public string UserName { get; set; } = null!;
}

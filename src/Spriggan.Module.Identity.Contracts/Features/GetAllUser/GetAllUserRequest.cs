using System.Text.Json.Serialization;
using Spriggan.Core;
using Spriggan.Core.Models;
using Spriggan.Core.Transport;

namespace Spriggan.Module.Identity.Contracts.Features.GetAllUser;

public class GetAllUserRequest : IRequest<Result<GetAllUserResponse>>, IPageable
{
    [JsonIgnore]
    public int PageNumber { get; set; } = 0;

    [JsonIgnore]
    public int PageSize { get; set; } = 50;
}

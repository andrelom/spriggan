using Spriggan.Module.Identity.Contracts.Models;

namespace Spriggan.Module.Identity.Contracts.Features.GetAllUser;

public class GetAllUserResponse
{
    public IEnumerable<User> Users { get; set; } = null!;
}

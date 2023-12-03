namespace Spriggan.Module.Identity.Contracts.Features.GetUserByName;

public class GetUserByNameResponse
{
    public Guid Id { get; set; }

    public string? UserName { get; set; }

    public string? Email { get; set; }
}

namespace Spriggan.Module.Identity.Contracts.Models;

public class User
{
    public Guid Id { get; set; }

    public string? Email { get; set; }

    public string? UserName { get; set; }
}

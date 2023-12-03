namespace Spriggan.Module.Identity.Contracts.Features.SignIn;

public class SignInResponse
{
    public Guid Id { get; set; }

    public string? AccessToken { get; set; }

    public DateTime ValidTo { get; set; }
}

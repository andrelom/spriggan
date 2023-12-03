namespace Spriggan.Module.Identity.Contracts;

public static class IdentityErrors
{
    //
    // Account

    public const string InvalidCredentials = nameof(InvalidCredentials);

    public const string TokenExpired = nameof(TokenExpired);

    public const string UserNotFound = nameof(UserNotFound);

    public const string UserNameAlreadyInUse = nameof(UserNameAlreadyInUse);

    public const string EmailNotConfirmed = nameof(EmailNotConfirmed);

    public const string EmailAlreadyInUse = nameof(EmailAlreadyInUse);
}

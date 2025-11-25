namespace BookHub.Features.Identity.Shared;

public static class Constants
{
    public const int AccountLockoutTimeSpan = 15;

    public const int MaxFailedLoginAttempts = 3;

    public const int DefaultTokenExpirationTime = 7;

    public const int ExtendedTokenExpirationTime = 30;

    public const string InvalidLoginAttempt = "Invalid log in attempt!";

    public const string InvalidRegisterAttempt = "Invalid register attempt!";

    public const string AccountWasLocked = "Account locked due to multiple failed attempts.";

    public const string AccountIsLocked = "Account is locked. Try again later.";
}

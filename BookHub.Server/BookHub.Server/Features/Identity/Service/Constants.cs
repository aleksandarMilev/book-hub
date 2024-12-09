namespace BookHub.Server.Features.Identity.Service
{
    public static class Constants
    {
        public const int DefaultTokenExpirationTime = 7;

        public const int ExtendedTokenExpirationTime = 30;

        public const string InvalidLoginAttempt = "Invalid log in attempt!";

        public const string InvalidRegisterAttempt = "Invalid register attempt!";

        public const string AccountWasLocked = "Account locked due to multiple failed attempts.";

        public const string AccountIsLocked = "Account is locked. Try again later.";
    }
}

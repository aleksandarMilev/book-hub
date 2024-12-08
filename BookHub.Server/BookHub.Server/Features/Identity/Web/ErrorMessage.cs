namespace BookHub.Server.Features.Identity.Web
{
    public static class ErrorMessage
    {
        public const string InvalidLoginAttempt = "Invalid log in attempt!";

        public const string AccountWasLocked = "Account locked due to multiple failed attempts.";

        public const string AccountIsLocked = "Account is locked. Try again later.";
    }
}

namespace BookHub.Features.Identity.Shared;

public static class Constants
{
    public static class ErrorMessages
    {
        public const string InvalidLoginAttempt = "Invalid log in attempt!";

        public const string InvalidRegisterAttempt = "Invalid register attempt!";

        public const string AccountWasLocked = "Account locked due to multiple failed attempts.";

        public const string AccountIsLocked = "Account is locked. Try again later.";
    }

    public static class Validation
    {
        public const int UsernameMinLength = 3;
        public const int UsernameMaxLength = 30;

        public const int PasswordMinLength = 8;
        public const int PasswordMaxLength = 128;

        public const int EmailMinLength = 5;
        public const int EmailMaxLength = 254;

        public const int MaxAgeYears = 110;
    }

    public static class Lockout
    {
        public const int AccountLockoutTimeSpan = 15;

        public const int MaxFailedLoginAttempts = 3;
    }

    public static class TokenExpiration
    {
        public const int DefaultTokenExpirationTime = 7;

        public const int ExtendedTokenExpirationTime = 30;
    }
}

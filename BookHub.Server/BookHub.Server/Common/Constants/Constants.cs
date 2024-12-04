namespace BookHub.Server.Common.Constants
{
    public static class Constants
    {
        public const string AdminRoleName = "Administrator";

        public const string AdminEmail = "admin@mail.com";

        public const string AdminPassword = "admin1234";

        public const int AccountLockoutTimeSpan = 15;

        public const int MaxFailedLoginAttempts = 3;

        public const int DefaultTokenExpirationTime = 7;

        public const int ExtendedTokenExpirationTime = 30;
    }
}

namespace BookHub.Features.UserProfile.Shared
{
    public static class ValidationConstants
    {
        public const int NameMinLength = 2;
        public const int NameMaxLength = 100;

        public const int UrlMinLength = 10;
        public const int UrlMaxLength = 2_000;

        public const int PhoneMinLength = 8;
        public const int PhoneMaxLength = 15;

        public const int BiographyMinLength = 10;
        public const int BiographyMaxLength = 1_000;

        public const int CurrentlyReadingBooksMaxCount = 5;
    }
}

namespace BookHub.Features.UserProfile.Shared;

public static class Constants
{
    public static class Validation
    {
        public const int UrlMinLength = 10;
        public const int UrlMaxLength = 2_000;

        public const int NameMinLength = 2;
        public const int NameMaxLength = 100;

        public const int BiographyMinLength = 10;
        public const int BiographyMaxLength = 1_000;

        public const int CurrentlyReadingBooksMaxCount = 5;
    }

    public static class Paths
    {
        public const string DefaultImagePath = "/images/profiles/default.jpg";

        public const string ProfilesImagePathPrefix = "profiles";
    }
}

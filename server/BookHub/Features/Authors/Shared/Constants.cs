namespace BookHub.Features.Authors.Shared;

public static class Constants
{
    public static class Validation
    {
        public const int NameMinLength = 2;
        public const int NameMaxLength = 200;

        public const int BiographyMinLength = 50;
        public const int BiographyMaxLength = 10_000;

        public const int ImageUrlMinLength = 10;
        public const int ImageUrlMaxLength = 2_000;

        public const int PenNameMinLength = 2;
        public const int PenNameMaxLength = 200;
    }

    public static class Paths
    {
        public const string DefaultImagePath = "/images/authors/default.jpg";

        public const string AuthorsImagePathPrefix = "authors";
    }
}

namespace BookHub.Features.Genres.Shared;

public static class Constants
{
    public static class Validation
    {
        public const int NameMinLength = 2;
        public const int NameMaxLength = 100;

        public const int DescriptionMinLength = 20;
        public const int DescriptionMaxLength = 3_000;

        public const int UrlMaxLength = 2_000;
    }
}

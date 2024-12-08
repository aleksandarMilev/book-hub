namespace BookHub.Server.Features.Genre.Shared
{
    public static class ValidationConstants
    {
        public const int NameMinLength = 2;
        public const int NameMaxLength = 100;

        public const int DescriptionMinLength = 20;
        public const int DescriptionMaxLength = 3_000;

        public const int UrlMaxLength = 2_000;
    }
}

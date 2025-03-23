namespace BookHub.Features.Article.Shared
{
    public static class ValidationConstants
    {
        public const int TitleMinLength = 10;
        public const int TitleMaxLength = 100;

        public const int IntroductionMinLength = 10;
        public const int IntroductionMaxLength = 500;

        public const int ContentMinLength = 100;
        public const int ContentMaxLength = 5_000;

        public const int UrlMinLength = 10;
        public const int UrlMaxLength = 2_000;
    }
}

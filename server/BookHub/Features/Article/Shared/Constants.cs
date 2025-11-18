namespace BookHub.Features.Article.Shared
{
    public static class Constants
    {
        public static class RouteNames
        {
            public const string DetailsRouteName = "ArticleDetails";
        }

        public static class DefaultValues
        {
            public const string DefaultImageUrl = "https://img.freepik.com/free-photo/bookmark-books-arrangement-top-view_23-2149894335.jpg";
        }

        public static class ValidationConstants
        {
            public const int TitleMinLength = 10;
            public const int TitleMaxLength = 100;

            public const int IntroductionMinLength = 10;
            public const int IntroductionMaxLength = 500;

            public const int ContentMinLength = 100;
            public const int ContentMaxLength = 50_000;

            public const int UrlMinLength = 10;
            public const int UrlMaxLength = 2_000;
        }
    }
}

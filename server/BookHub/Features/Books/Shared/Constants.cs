namespace BookHub.Features.Books.Shared;

public static class Constants
{
    public static class Validation
    {
        public const int ShortDescriptionMinLength = 10;
        public const int ShortDescriptionMaxLength = 200;

        public const int LongDescriptionMinLength = 100;
        public const int LongDescriptionMaxLength = 10_000;

        public const int AuthorNameMinLength = 2;
        public const int AuthorNameMaxLength = 100;

        public const int ImageUrlMinLength = 10;
        public const int ImageUrlMaxLength = 2_000;

        public const int TitleMinLength = 2;
        public const int TitleMaxLength = 200;

        public const double RatingMinValue = 1.0;
        public const double RatingMaxValue = 5.0;
    }


    public static class Paths
    {
        public const string DefaultImagePath = $"/images/{ImagePathPrefix}/default.jpg";

        public const string ImagePathPrefix = "books";
    }

    public static class RouteNames
    {
        public const string DetailsRouteName = "BookDetails";
    }
}

namespace BookHub.Features.Review.Shared
{
    public static class Constants
    {
        public static class Validation
        {
            public const int ContentMinLength = 5;
            public const int ContentMaxLength = 5_000;

            public const int RatingMinValue = 1;
            public const int RatingMaxValue = 5;
        }
    }
}

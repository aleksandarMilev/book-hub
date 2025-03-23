namespace BookHub.Features.Review.Shared
{
    public static class ValidationConstants
    {
        public const int ContentMinLength = 5;
        public const int ContentMaxLength = 5_000;

        public const double RatingMinValue = 1.0;
        public const double RatingMaxValue = 5.0;
    }
}

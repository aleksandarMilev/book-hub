namespace BookHub.Features.Reviews.Shared;

public static class Constants
{
    public static class Validation
    {
        public const int ContentMinLength = 5;
        public const int ContentMaxLength = 5_000;

        public const int RatingMinValue = 1;
        public const int RatingMaxValue = 5;
    }

    public static class RouteNames
    {
        public const string DetailsRouteName = "ReviewDetails";
    }

    public static class ErrorMessages
    {
        public const string BookDoesNotExist = "The book does not exist.";

        public const string AlreadyReviewed = "You have already reviewed this book.";

        public const string BookIdCannotChange = "The book of a review cannot be changed.";
    }
}

namespace BookHub.Features.ReadingList.Shared;

public static class Constants
{
    public static class ErrorMessages
    {
        public const string BookAlreadyInTheList = "This book is already added in the user list!";

        public const string BookNotInTheList = "The book has not been found in the user list!";

        public const string MoreThanFiveCurrentlyReading = "User can not add more than 5 books in the currently reading list!";
    }
}

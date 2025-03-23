namespace BookHub.Features.ReadingList.Service
{
    public static class ErrorMessage
    {
        public const string BookAlreadyInTheList = "This book is already added in the user list!";

        public const string BookNotInTheList = "The book has not been found in the user list!";

        public const string MoreThanFiveCurrentlyReading = "User can not add more than 5 books in the currently reading list!";
    }
}

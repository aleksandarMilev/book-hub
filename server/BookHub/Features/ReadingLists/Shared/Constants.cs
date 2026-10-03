namespace BookHub.Features.ReadingLists.Shared;

public static class Constants
{
    public static class ErrorMessages
    {
        public const string BookAlreadyInTheList = "This book is already in the list with this status.";

        public const string BookNotInTheList = "The book is not in your reading list.";

        public const string BookDoesNotExist = "The book does not exist.";

        public const string InvalidStatus = "Invalid reading status.";

        // Also used when the profile is private, so the response doesn't reveal whether it exists (S-04).
        public const string ReadingListNotFound = "The reading list was not found.";
    }
}

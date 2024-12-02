namespace BookHub.Server.Common.Messages
{
    public static class Error
    {
        public static class Book
        {
            public const string BookNotFound = "The book was not found!";

            public const string UnauthorizedBookEdit = "Current user can not edit this book!";

            public const string UnauthorizedBookDelete = "Current user can not delete this book!";
        }

        public static class Author
        {
            public const string AuthorNotFound = "The author was not found!";

            public const string UnauthorizedAuthorEdit = "Current user can not edit this author!";

            public const string UnauthorizedAuthorDelete = "Current user can not delete this author!";
        }

        public static class Review
        {
            public const string ReviewNotFound = "The review was not found!";

            public const string UnauthorizedReviewEdit = "Current user can not edit this review!";

            public const string UnauthorizedReviewDelete = "Current user can not delete this review!";
        }

        public static class Article
        {
            public const string ArticleNotFound = "The article was not found!";

            public const string UnauthorizedArticleEdit = "Current user can not edit this article!";

            public const string UnauthorizedArticleDelete = "Current user can not delete this article!";
        }


        public static class Profile
        {
            public const string ProfileNotFound = "The profile was not found!";
        }

        public static class Identity
        {
            public const string InvalidLoginAttempt = "Invalid log in attempt!";
        }

        public static class Notification
        {
            public const string NotificationNotFound = "The notification was not found!";
        }

        public static class ReadingList
        {
            public const string BookAlreadyInTheList = "This book is already added in the user list!";

            public const string BookNotInTheList = "The book has not been found in the user list!";

            public const string MoreThanFiveCurrentlyReading = "User can not add more than 5 books in the currently reading list!";
        }
    }
}

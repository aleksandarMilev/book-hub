﻿namespace BookHub.Server.Common.Messages
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

        public static class Identity
        {
            public const string InvalidLoginAttempt = "Invalid log in attempt!";
        }

        public static class Vote
        {
            public const string VoteExists = "This user has already voted!";
        }
    }
}

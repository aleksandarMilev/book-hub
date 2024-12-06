﻿namespace BookHub.Server.Common.Constants
{
    public static class Validation
    {
        public static class Book
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

        public static class Author
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 200;

            public const int BiographyMinLength = 50;
            public const int BiographyMaxLength = 10_000;

            public const int ImageUrlMinLength = 10;
            public const int ImageUrlMaxLength = 2_000;

            public const int PenNameMinLength = 2;
            public const int PenNameMaxLength = 200;
        }

        public static class Review
        {
            public const int ContentMinLength = 5;
            public const int ContentMaxLength = 5_000;

            public const double RatingMinValue = 1.0;
            public const double RatingMaxValue = 5.0;
        }

        public static class Reply
        {
            public const int ContentMinLength = 5;
            public const int ContentMaxLength = 1_000;
        } 
        
        public static class Genre
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 100;

            public const int DescriptionMinLength = 20;
            public const int DescriptionMaxLength = 3_000;

            public const int UrlMaxLength = 2_000;
        }

        public static class Nationality
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 100;
        }

        public static class Profile
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 100;

            public const int UrlMinLength = 10;
            public const int UrlMaxLength = 2_000;

            public const int PhoneMinLength = 8;
            public const int PhoneMaxLength = 15;

            public const int BiographyMinLength = 10;
            public const int BiographyMaxLength = 1_000;

            public const int CurrentlyReadingBooksMaxCount = 5;
        }

        public static class Article
        {
            public const int TitleMinLength = 10;
            public const int TitleMaxLength = 100;

            public const int IntroductionMinLength = 10;
            public const int IntroductionMaxLength = 500;

            public const int ContentMinLength = 100;
            public const int ContentMaxLength = 5_000;

            public const int UrlMinLength = 10;
            public const int UrlMaxLength = 2_000;
        }

        public static class Notification
        {
            public const int MessageMinLength = 10;
            public const int MessageMaxLength = 500;
        }

        public static class Chat
        {
            public const int MessageMinLength = 1;
            public const int MessageMaxLength = 5_000;

            public const int NameMinLength = 1;
            public const int NameMaxLength = 200;

            public const int UrlMinLength = 10;
            public const int UrlMaxLength = 2_000;
        }
    }
}

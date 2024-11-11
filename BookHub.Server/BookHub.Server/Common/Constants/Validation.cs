namespace BookHub.Server.Common.Constants
{
    public static class Validation
    {
        public static class Book
        {
            public const int ShortDescriptionMinLength = 10;
            public const int ShortDescriptionMaxLength = 100;

            public const int LongDescriptionMinLength = 100;
            public const int LongDescriptionMaxLength = 5_000;

            public const int AuthorMinLength = 2;
            public const int AuthorMaxLength = 100;

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
            public const int ContentMaxLength = 1_000;
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
        }

        public static class Nationality
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 100;
        }
    }
}

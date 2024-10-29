namespace BookHub.Server.Data.Common
{
    public static class Validation
    {
        public static class Book 
        {
            public const int DescriptionMinLength = 10;
            public const int DescriptionMaxLength = 2_000;

            public const int AuthorMinLength = 2;
            public const int AuthorMaxLength = 100;

            public const int ImageUrlMinLength = 10;
            public const int ImageUrlMaxLength = 2_000;

            public const int TitleMinLength = 2;
            public const int TitleMaxLength = 200;
        }
    }
}

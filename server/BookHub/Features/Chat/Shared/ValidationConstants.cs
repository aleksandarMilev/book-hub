namespace BookHub.Features.Chat.Shared
{
    public class ValidationConstants
    {
        public const int MessageMinLength = 1;
        public const int MessageMaxLength = 5_000;

        public const int NameMinLength = 1;
        public const int NameMaxLength = 200;

        public const int UrlMinLength = 10;
        public const int UrlMaxLength = 2_000;
    }
}

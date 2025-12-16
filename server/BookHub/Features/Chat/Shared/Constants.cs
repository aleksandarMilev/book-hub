namespace BookHub.Features.Chat.Shared;

public static class Constants
{
    public static class Validation
    {
        public const int MessageMinLength = 1;
        public const int MessageMaxLength = 5_000;

        public const int NameMinLength = 1;
        public const int NameMaxLength = 200;
    }

    public static class Paths
    {
        public const string DefaultImagePath = "/images/chats/default.jpg";

        public const string ImagePathPrefix = "chats";
    }
}

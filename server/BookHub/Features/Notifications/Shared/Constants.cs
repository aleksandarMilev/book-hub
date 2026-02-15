namespace BookHub.Features.Notifications.Shared;

public static class Constants
{
    public static class Validation
    {
        public const int MessageMinLength = 10;
        public const int MessageMaxLength = 500;
    }

    public static class Messages
    {
        public const string Created = "{0} has created '{1}'";

        public const string Edited = "{0} has edited '{1}'";

        public const string Approved = "'{0}' has been approved";

        public const string Rejected = "'{0}' has been rejected";

        public const string ChatInvitation = "{0} has invited you to join in '{1}'";

        public const string ChatInvitationAccepted = "{0} accepted to join in '{1}'";

        public const string ChatInvitationRejected = "{0} rejected to join in '{1}'";
    }
}

namespace BookHub.Features.Notification.Service
{
    public static class Constants
    {
        public static class Messages
        {
            public const string Created = "{0} has created '{1}'";

            public const string ApprovalStatusChange = "'{0}' has been {1}";

            public const string ChatInvitation = "{0} has invited you to join in '{1}'";

            public const string ChatInvitationStatusChange = "{0} has {1} to join in '{2}'";
        }

        public const string Approved = "approved";

        public const string Accepted = "accepted";

        public const string Rejected = "rejected";
    }
}

namespace BookHub.Features.Chat.Web
{
    public static class ApiRoutes
    {
        public const string NotJoined = "not-joined/";

        public const string Access = "access/{userId}/";

        public const string Invited = "invited/{userId}/";

        public const string Invite = "invite/";

        public const string AcceptInvite = "invite/accept/";

        public const string RejectInvite = "invite/reject/";

        public const string RemoveUser = "remove/user/";
    }
}

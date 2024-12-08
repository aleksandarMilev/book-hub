namespace BookHub.Server.Common
{
    public static class ErrorMessage
    {
        public const string DbEntityNotFound = "{0} with Id: {1} was not found!";

        public const string UnauthorizedDbEntityAction = "{0} can not modify {1} with Id: {2}!";
    }
}

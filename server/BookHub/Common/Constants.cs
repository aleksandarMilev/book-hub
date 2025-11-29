namespace BookHub.Common;

public static class Constants
{
    public static class DefaultValues
    {
        public const int DefaultPageIndex = 1;

        public const int DefaultPageSize = 10;
    }

    public static class ApiRoutes
    {
        public const string Id = "{id}/";
    }

    public static class ErrorMessages
    {
        public const string DbEntityNotFound = "{0} with Id: {1} was not found!";

        public const string DbEntityNotFoundTemplate = $"{{Entity}} with Id: {{Id}} was not found!";

        public const string UnauthorizedMessage = "User with Id: {0} can not modify {1} with Id: {2}!";

        public const string UnauthorizedMessageTemplate = $"User with Id: {{UserId}} can not modify {{ResourceName}} with Id: {{ResourceId}}!";

        //TODO: remove
        public const string UnauthorizedDbEntityAction = "{0} can not modify {1} with Id: {2}!";
    }

    public static class Names 
    {
        public const string AdminRoleName = "Administrator";
    }

    public static class Cors
    {
        public const string CorsPolicyName = "CorsPolicy";
    }
}

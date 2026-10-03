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
        // Log templates: they keep the internal type name and the IDs, and never reach clients.
        public const string DbEntityNotFoundTemplate = $"{{Entity}} with Id: {{Id}} was not found!";

        public const string UnauthorizedMessageTemplate = $"User with Id: {{UserId}} can not modify {{ResourceName}} with Id: {{ResourceId}}!";

        // Returned to clients (ProblemDetails.detail): a friendly resource name only,
        // never a type name or an ID (S-11). {0} is e.g. "book" or "review".
        public const string ResourceNotFound = "The {0} was not found.";

        public const string ResourceForbidden = "You are not allowed to modify this {0}.";
    }

    public static class Names 
    {
        public const string AdminRoleName = "Administrator";
    }

    public static class Cors
    {
        public const string CorsPolicyName = "CorsPolicy";
    }

    public static class DateFormats
    {
        public const string ISO8601 = "yyyy-MM-dd";
    }

    public static class Validation
    {
        public const int ImagePathMaxLength = 512;
    }
}

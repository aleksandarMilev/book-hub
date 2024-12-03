namespace BookHub.Server.Features.Search.Service.Models
{
    public class SearchProfileServiceModel
    {
        public string Id { get; init; } = null!;

        public string FirstName { get; init; } = null!;

        public string LastName { get; init; } = null!;

        public string ImageUrl { get; init; } = null!;

        public bool IsPrivate { get; init; }
    }
}

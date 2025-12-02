namespace BookHub.Features.Search.Service.Models
{
    public class SearchArticleServiceModel
    {
        public Guid Id { get; init; }

        public string Title { get; init; } = null!;

        public string Introduction { get; init; } = null!;

        public int Views { get; set; }

        public string ImagePath { get; init; } = null!;

        public DateTime CreatedOn { get; init; }
    }
}

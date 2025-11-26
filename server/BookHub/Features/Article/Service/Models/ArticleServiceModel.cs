namespace BookHub.Features.Article.Service.Models;

public abstract class ArticleServiceModel
{
    public string Title { get; init; } = null!;

    public string Introduction { get; init; } = null!;

    public string Content { get; init;} = null!;
}

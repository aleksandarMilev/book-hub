namespace BookHub.Features.Articles.Service.Models.Base;

public abstract class ArticleBaseModel
{
    public string Title { get; init; } = null!;

    public string Introduction { get; init; } = null!;

    public string Content { get; init; } = null!;
}

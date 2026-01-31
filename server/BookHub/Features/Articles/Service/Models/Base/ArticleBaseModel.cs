namespace BookHub.Features.Articles.Service.Models.Base;

public abstract class ArticleBaseModel
{
    public string Title { get; init; } = default!;

    public string Introduction { get; init; } = default!;

    public string Content { get; init; } = default!;
}

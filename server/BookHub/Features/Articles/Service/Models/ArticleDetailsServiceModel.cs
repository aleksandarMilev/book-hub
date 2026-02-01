namespace BookHub.Features.Articles.Service.Models;

using Base;

public class ArticleDetailsServiceModel : ArticleBaseModel
{
    public Guid Id { get; init; }

    public int Views { get; init; }

    public string ImagePath { get; init; } = default!;

    public DateTime CreatedOn { get; init; }

    public DateTime? ModifiedOn { get; init; }
}

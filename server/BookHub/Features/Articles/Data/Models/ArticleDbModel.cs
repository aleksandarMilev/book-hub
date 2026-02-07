namespace BookHub.Features.Articles.Data.Models;

using BookHub.Data.Models.Base;
using Infrastructure.Services.ImageWriter.Models;

public class ArticleDbModel :
    DeletableEntity<Guid>,
    IImageDdModel
{
    public string Title { get; set; } = default!;

    public string Introduction { get; set; } = default!;

    public string Content { get; set; } = default!;

    public string ImagePath { get; set; } = default!;

    public int Views { get; init; }
}

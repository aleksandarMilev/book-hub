namespace BookHub.Features.Books.Data.Models;

using BookHub.Data.Models.Base;
using Infrastructure.Services.ImageWriter.Models;

public class BookEditDbModel :
    DeletableEntity<Guid>,
    IImageDdModel
{
    public Guid BookId { get; set; }

    public BookDbModel Book { get; set; } = default!;

    public string RequestedById { get; set; } = default!;

    public string Title { get; set; } = default!;

    public string ShortDescription { get; set; } = default!;

    public string LongDescription { get; set; } = default!;

    public DateTime? PublishedDate { get; set; }

    public string ImagePath { get; set; } = default!;

    public Guid? AuthorId { get; set; }

    public string GenresJson { get; set; } = "[]";
}

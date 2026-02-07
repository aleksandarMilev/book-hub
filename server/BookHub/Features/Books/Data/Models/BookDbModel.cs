namespace BookHub.Features.Books.Data.Models;

using Authors.Data.Models;
using BookHub.Data.Models.Base;
using BookHub.Data.Models.Shared.BookGenre.Models;
using Identity.Data.Models;
using Infrastructure.Services.ImageWriter.Models;
using ReadingLists.Data.Models;
using Reviews.Data.Models;

public class BookDbModel:
    DeletableEntity<Guid>,
    IApprovableEntity,
    IImageDdModel
{
    public string Title { get; set; } = default!;

    public string ShortDescription { get; set; } = default!;

    public string LongDescription { get; set; } = default!;

    public double AverageRating { get; set; }

    public int RatingsCount { get; set; }

    public DateTime? PublishedDate { get; set; }

    public string ImagePath { get; set; } = default!;

    public Guid? AuthorId { get; set; }

    public AuthorDbModel? Author { get; set; }

    public string? CreatorId { get; set; }

    public UserDbModel? Creator { get; set; }

    public bool IsApproved { get; set; }

    public ICollection<BookGenreDbModel> BooksGenres { get; set; } = new HashSet<BookGenreDbModel>();

    public ICollection<ReviewDbModel> Reviews { get; set; } = new HashSet<ReviewDbModel>();

    public ICollection<ReadingListDbModel> ReadingLists { get; set; } = new HashSet<ReadingListDbModel>();
}

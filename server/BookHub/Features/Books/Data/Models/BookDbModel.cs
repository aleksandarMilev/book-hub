namespace BookHub.Features.Books.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Authors.Data.Models;
using BookHub.Data.Models.Base;
using BookHub.Data.Models.Shared.BookGenre.Models;
using Identity.Data.Models;
using Infrastructure.Services.ImageWriter.Models.Image;
using ReadingLists.Data.Models;
using Reviews.Data.Models;

using static Shared.Constants.Validation;

public class BookDbModel:
    DeletableEntity<Guid>,
    IApprovableEntity,
    IImageDdModel
{
    [Required]
    [MaxLength(TitleMaxLength)]
    public string Title { get; set; } = null!;

    [Required]
    [MaxLength(ShortDescriptionMaxLength)]
    public string ShortDescription { get; set; } = null!;

    [Required]
    [MaxLength(LongDescriptionMaxLength)]
    public string LongDescription { get; set; } = null!;

    public double AverageRating { get; set; }

    public int RatingsCount { get; set; }

    public DateTime? PublishedDate { get; set; }

    [Required]
    public string ImagePath { get; set; } = null!;

    [ForeignKey(nameof(Author))]
    public Guid? AuthorId { get; set; }

    public AuthorDbModel? Author { get; set; }

    [ForeignKey(nameof(UserDbModel))]
    public string? CreatorId { get; set; }

    public UserDbModel? Creator { get; set; }

    public bool IsApproved { get; set; }

    public ICollection<BookGenreDbModel> BooksGenres { get; set; } = new HashSet<BookGenreDbModel>();

    public ICollection<ReviewDbModel> Reviews { get; set; } = new HashSet<ReviewDbModel>();

    public ICollection<ReadingListDbModel> ReadingLists { get; set; } = new HashSet<ReadingListDbModel>();
}

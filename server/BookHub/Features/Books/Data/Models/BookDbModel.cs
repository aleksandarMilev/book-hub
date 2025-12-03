namespace BookHub.Features.Book.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookHub.Data.Models.Base;
using BookHub.Data.Models.Shared.BookGenre.Models;
using Features.Authors.Data.Models;
using Features.Identity.Data.Models;
using Features.ReadingList.Data.Models;
using Features.Review.Data.Models;
using Infrastructure.Services.ImageWriter.Models.Image;

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

    [ForeignKey(nameof(User))]
    public string? CreatorId { get; set; }

    public User? Creator { get; set; }

    public bool IsApproved { get; set; }

    public ICollection<BookGenreDbModel> BooksGenres { get; set; } = new HashSet<BookGenreDbModel>();

    public ICollection<Review> Reviews { get; set; } = new HashSet<Review>();

    public ICollection<ReadingList> ReadingLists { get; set; } = new HashSet<ReadingList>();
}

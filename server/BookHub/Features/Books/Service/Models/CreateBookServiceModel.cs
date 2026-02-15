namespace BookHub.Features.Books.Service.Models;

using Infrastructure.Services.ImageWriter.Models;

public class CreateBookServiceModel : IImageServiceModel
{
    public string Title { get; init; } = default!;

    public Guid? AuthorId { get; init; }

    public IFormFile? Image { get; init; }

    public string ShortDescription { get; init; } = default!;

    public string LongDescription { get; init; } = default!;

    public int? Pages { get; set; }

    public DateTime? PublishedDate { get; init; }

    public ICollection<Guid> Genres { get; init; } = new HashSet<Guid>();
}

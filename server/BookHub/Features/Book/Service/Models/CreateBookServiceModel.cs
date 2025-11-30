namespace BookHub.Features.Book.Service.Models;

using Infrastructure.Services.ImageWriter.Models.Image;

public class CreateBookServiceModel : IImageServiceModel
{
    public string Title { get; init; } = null!;

    public Guid? AuthorId { get; init; }

    public IFormFile? Image { get; init; }

    public string ShortDescription { get; init; } = null!;

    public string LongDescription { get; init; } = null!;

    public string? PublishedDate { get; init; }

    public ICollection<int> Genres { get; init; } = new HashSet<int>();
}

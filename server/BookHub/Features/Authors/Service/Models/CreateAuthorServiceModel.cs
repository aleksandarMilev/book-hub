namespace BookHub.Features.Authors.Service.Models;

using Infrastructure.Services.ImageWriter.Models;
using Shared;

public class CreateAuthorServiceModel : IImageServiceModel
{
    public string Name { get; init; } = default!;

    public IFormFile? Image { get; init; }

    public string Biography { get; init; } = default!;

    public string? PenName { get; init; }

    public Nationality Nationality { get; init; }

    public Gender Gender { get; init; }

    public DateTime? BornAt { get; init; }

    public DateTime? DiedAt { get; init; }
}

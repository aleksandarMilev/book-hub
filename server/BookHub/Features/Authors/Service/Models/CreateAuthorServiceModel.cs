namespace BookHub.Features.Authors.Service.Models;

using Infrastructure.Services.ImageWriter.Models.Image;
using Shared;

public class CreateAuthorServiceModel : IImageServiceModel
{
    public string Name { get; init; } = null!;

    public IFormFile? Image { get; init; }

    public string Biography { get; init; } = null!;

    public string? PenName { get; init; }

    public Nationality Nationality { get; init; }

    public Gender Gender { get; init; }

    public string? BornAt { get; init; }

    public string? DiedAt { get; init; }
}

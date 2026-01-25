namespace BookHub.Infrastructure.Services.ImageWriter;

using Models.Image;
using ServiceLifetimes;

public interface IImageWriter : IScopedService
{
    Task Write(
        string resourceName,
        IImageDdModel dbModel,
        IImageServiceModel serviceModel,
        string? defaultImagePath = null,
        CancellationToken cancellationToken = default);

    bool Delete(
        string resourceName,
        string? imagePath,
        string? defaultImagePath = null);
}

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
        CancellationToken token = default);

    void Delete(
        string resourceName,
        string? imagePath,
        string? defaultImagePath = null);
}

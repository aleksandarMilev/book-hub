namespace BookHub.Infrastructure.Services.ImageWriter;

using Common.Models.Image;
using Result;

public class ImageWriter(
    ILogger<ImageWriter> logger,
    IWebHostEnvironment env) : IImageWriter
{
    private const long MaxImageSizeBytes = 2 * 1_024 * 1_024;

    private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".webp", ".avif"];
    private static readonly string[] AllowedContentTypes = ["image/jpeg", "image/png", "image/webp", "image/avif"];

    public async Task Write<TId>(
        TId id,
        string resourceName,
        IImageDdModel dbModel,
        IImageServiceModel serviceModel,
        string? defaultImagePath = null,
        CancellationToken token = default)
    {
        if (serviceModel.Image is not null)
        {
            var validationResult = ValidateImageFile(serviceModel.Image);
            if (!validationResult.Succeeded)
            {
                logger.LogWarning(
                    "Invalid image upload for {resourceName} with id: {Id}: {Error}",
                    resourceName,
                    id,
                    validationResult.ErrorMessage);

                throw new InvalidOperationException(validationResult.ErrorMessage);
            }

            await this.SaveImageFile(resourceName, dbModel, serviceModel, token);
        }
        else 
        {
            if (defaultImagePath is not null)
            {
                dbModel.ImagePath = defaultImagePath;
            }
        }
    }

    public void Delete<TId>(
        TId id,
        string resourceName,
        string? imagePath,
        string? defaultImagePath = null)
    {
        if (string.IsNullOrWhiteSpace(imagePath))
        {
            return;
        }

        if (!string.IsNullOrWhiteSpace(defaultImagePath) &&
            string.Equals(imagePath, defaultImagePath, StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        if (Uri.TryCreate(imagePath, UriKind.Absolute, out _))
        {
            return;
        }

        try
        {
            var relativePath = imagePath.TrimStart('/', '\\');
            var physicalPath = Path.Combine(
                env.WebRootPath,
                relativePath.Replace('/', Path.DirectorySeparatorChar));

            if (File.Exists(physicalPath))
            {
                File.Delete(physicalPath);
                logger.LogInformation(
                    "Deleted image file for {resourceName} with Id: {id} at path: {Path}",
                    resourceName,
                    id,
                    physicalPath);
            }
        }
        catch (Exception exception)
        {
            logger.LogWarning(
                exception,
                "Failed to delete image file for {resourceName} with Id: {id} at path: {Path}",
                resourceName,
                id,
                imagePath);
        }
    }

    private static Result ValidateImageFile(IFormFile image)
    {
        if (image.Length == 0)
        {
            return "Image file is empty.";
        }

        if (image.Length > MaxImageSizeBytes)
        {
            return $"Image must be smaller than {MaxImageSizeBytes / 1_024 / 1_024} MB.";
        }

        var extension = Path.GetExtension(image.FileName).ToLowerInvariant();
        if (!AllowedExtensions.Contains(extension))
        {
            return $"Invalid image extension. Allowed: {string.Join(", ", AllowedExtensions)}.";
        }

        if (!AllowedContentTypes.Contains(image.ContentType))
        {
            return $"Invalid image content type. Allowed: {string.Join(", ", AllowedContentTypes)}.";
        }

        return true;
    }

    private async Task SaveImageFile(
        string resourceName,
        IImageDdModel dbModel,
        IImageServiceModel serviceModel,
        CancellationToken token = default)
    {
        var extension = Path.GetExtension(serviceModel.Image!.FileName).ToLowerInvariant();
        var fileName = $"{Guid.NewGuid()}{extension}";
        var uploadsRoot = Path.Combine(env.WebRootPath, "images", resourceName);

        Directory.CreateDirectory(uploadsRoot);

        var filePath = Path.Combine(uploadsRoot, fileName);

        try
        {
            await using var stream = new FileStream(filePath, FileMode.Create);
            await serviceModel.Image.CopyToAsync(stream, token);

            dbModel.ImagePath = $"/images/{resourceName}/{fileName}";
        }
        catch (Exception exception)
        {
            logger.LogError(
                exception,
                "Error saving {resourceName} image to path {Path}",
                resourceName,
                filePath);

            throw;
        }
    }
}

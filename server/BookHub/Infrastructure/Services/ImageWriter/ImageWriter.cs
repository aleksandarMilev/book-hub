namespace BookHub.Infrastructure.Services.ImageWriter;

using Models;

using static Common.Utils;

public class ImageWriter(
    ILogger<ImageWriter> logger,
    IWebHostEnvironment env) : IImageWriter
{
    private const string ImagesPathPrefix = "images";

    public async Task Write(
        string resourceName,
        IImageDdModel dbModel,
        IImageServiceModel serviceModel,
        string? defaultImagePath = null,
        CancellationToken cancelationToken = default)
    {
        if (serviceModel.Image is not null)
        {
            var validationResult = ValidateImageFile(serviceModel.Image);
            if (!validationResult.Succeeded)
            {
                logger.LogWarning(
                    "Invalid image upload for {resourceName}. Error: {Error}.",
                    resourceName,
                    validationResult.ErrorMessage);

                throw new InvalidOperationException(validationResult.ErrorMessage);
            }

            await this.SaveImageFile(
                resourceName,
                dbModel,
                serviceModel,
                cancelationToken);
        }
        else 
        {
            if (defaultImagePath is not null)
            {
                dbModel.ImagePath = defaultImagePath;
            }
        }
    }

    public bool Delete(
        string resourceName,
        string? imagePath,
        string? defaultImagePath = null)
    {
        if (string.IsNullOrWhiteSpace(imagePath))
        {
            logger.LogInformation(
                "Delete skipped for {Resource}: empty imagePath.",
                resourceName);

            return false;
        }

        var isDefaultImagePath = 
            !string.IsNullOrWhiteSpace(defaultImagePath) &&
            string.Equals(
                imagePath,
                defaultImagePath,
                StringComparison.OrdinalIgnoreCase);

        if (isDefaultImagePath)
        {
            logger.LogInformation(
                "Delete skipped for {Resource}: imagePath is default. Path={Path}",
                resourceName,
                imagePath);

            return false;
        }

        if (imagePath.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
            imagePath.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            logger.LogInformation("Delete skipped for {resourceName}: remote URL. Path={Path}", resourceName, imagePath);
            return false;
        }

        var relativePath = imagePath.TrimStart('/', '\\');
        var physicalPath = Path.Combine(
            env.WebRootPath,
            relativePath.Replace('/', Path.DirectorySeparatorChar));

        if (!File.Exists(physicalPath))
        {
            var fileName = Path.GetFileName(relativePath);
            var fallback = Path.Combine(
                env.WebRootPath,
                ImagesPathPrefix,
                resourceName,
                fileName);

            logger.LogWarning(
                "Delete: file not found at primary path. Resource={Resource}, Primary={Primary}, Fallback={Fallback}, ExistsFallback={ExistsFallback}",
                resourceName,
                physicalPath,
                fallback,
                File.Exists(fallback));

            physicalPath = fallback;
        }

        var exists = File.Exists(physicalPath);

        logger.LogInformation(
            "Delete attempt. Resource={Resource}, ImagePath={ImagePath}, WebRoot={WebRoot}, Resolved={Resolved}, Exists={Exists}",
            resourceName,
            imagePath,
            env.WebRootPath,
            physicalPath,
            exists);

        if (!exists)
        {
            return false;
        }

        try
        {
            File.Delete(physicalPath);
        }
        catch (Exception exception)
        {
            logger.LogWarning(
                exception,
                "Failed to delete image file. Resource={Resource}, Resolved={Resolved}",
                resourceName,
                physicalPath);

            return false;
        }

        logger.LogInformation(
            "Deleted image file. Resource={Resource}, Resolved={Resolved}",
            resourceName,
            physicalPath);

        return true;
    }

    private async Task SaveImageFile(
        string resourceName,
        IImageDdModel dbModel,
        IImageServiceModel serviceModel,
        CancellationToken token = default)
    {
        var extension = Path
            .GetExtension(serviceModel.Image!.FileName)
            .ToLowerInvariant();

        var fileName = $"{Guid.NewGuid()}{extension}";

        var uploadsRoot = Path.Combine(
            env.WebRootPath,
            ImagesPathPrefix,
            resourceName);

        Directory.CreateDirectory(uploadsRoot);

        var filePath = Path.Combine(uploadsRoot, fileName);

        try
        {
            await using var stream = new FileStream(filePath, FileMode.Create);
            await serviceModel.Image.CopyToAsync(stream, token);

            dbModel.ImagePath = $"/{ImagesPathPrefix}/{resourceName}/{fileName}";
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

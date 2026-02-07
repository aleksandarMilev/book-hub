namespace BookHub.Tests.Shared.Mocks;

using Infrastructure.Services.ImageWriter;
using Infrastructure.Services.ImageWriter.Models;

public sealed class ImageWriterMock : IImageWriter
{
    public int WriteCalls { get; private set; }

    public int DeleteCalls { get; private set; }

    public string? LastDeletedPath { get; private set; }

    public string? LastWrittenPath { get; private set; }

    public Task Write(
        string resourceName,
        IImageDdModel dbModel,
        IImageServiceModel serviceModel,
        string? defaultImagePath = null,
        CancellationToken cancellationToken = default)
    {
        this.WriteCalls++;

        if (serviceModel.Image is null)
        {
            if (defaultImagePath is not null)
            {
                dbModel.ImagePath = defaultImagePath;
                this.LastWrittenPath = defaultImagePath;
            }

            return Task.CompletedTask;
        }

        var newPath = $"/images/{resourceName}/test-{Guid.NewGuid():N}.jpg";

        dbModel.ImagePath = newPath;
        this.LastWrittenPath = newPath;

        return Task.CompletedTask;
    }

    public bool Delete(
        string resourceName,
        string? imagePath,
        string? defaultImagePath = null)
    {
        this.DeleteCalls++;
        this.LastDeletedPath = imagePath;

        return true;
    }
}

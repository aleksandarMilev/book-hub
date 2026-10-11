namespace BookHub.Tests.Shared.Mocks;

using Infrastructure.Services.ImageWriter;
using Infrastructure.Services.ImageWriter.Models;

public sealed class ImageWriterMock : IImageWriter
{
    public int WriteCalls { get; private set; }

    public int DeleteCalls { get; private set; }

    public string? LastDeletedPath { get; private set; }

    public string? LastWrittenPath { get; private set; }

    // When set, writes for this resource (e.g. "profiles") throw, as a full disk would.
    public string? FailWritesFor { get; set; }

    public Task Write(
        string resourceName,
        IImageDdModel dbModel,
        IImageServiceModel serviceModel,
        string? defaultImagePath = null,
        CancellationToken cancellationToken = default)
    {
        this.WriteCalls++;

        if (string.Equals(resourceName, this.FailWritesFor, StringComparison.Ordinal))
        {
            throw new IOException($"Simulated failure writing a {resourceName} image.");
        }

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

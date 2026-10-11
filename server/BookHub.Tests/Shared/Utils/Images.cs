namespace BookHub.Tests.Shared.Utils;

using System.Net.Http.Headers;

// Tiny but real images for upload tests: the app's real ImageValidator checks the size, the
// extension, the content type and the magic bytes, and the real ImageWriter writes them to disk.
// The arrays are rebuilt on every access, so no test can change another's bytes.
public static class Images
{
    // 1x1 PNG.
    private const string PngBase64 = "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mP8/x8AAwMCAO6qY9kAAAAASUVORK5CYII=";

    // 1x1 baseline JPEG.
    private const string JpegBase64 = "/9j/4AAQSkZJRgABAQEASABIAAD/2wBDAP//////////////////////////////////////////////////////////////////////////////////////wgALCAABAAEBAREA/8QAFBABAAAAAAAAAAAAAAAAAAAAAP/aAAgBAQABPxA=";

    // Just over ImageValidator's 2 MB limit.
    private const int TooLargeBytes = (2 * 1_024 * 1_024) + 1;

    public static byte[] Png => Convert.FromBase64String(PngBase64);

    public static byte[] Jpeg => Convert.FromBase64String(JpegBase64);

    // Adds a valid JPEG as the "Image" field of a form.
    public static MultipartFormDataContent WithImage(
        this MultipartFormDataContent form,
        string fileName = "cover.jpg")
        => form.WithFile(Jpeg, fileName, "image/jpeg");

    public static MultipartFormDataContent WithFile(
        this MultipartFormDataContent form,
        byte[] bytes,
        string fileName,
        string contentType,
        string fieldName = "Image")
    {
        var content = new ByteArrayContent(bytes);
        content.Headers.ContentType = new MediaTypeHeaderValue(contentType);

        form.Add(content, fieldName, fileName);

        return form;
    }

    // Each upload the validator must reject, with the start of the message it returns.
    public static TheoryData<string, string, string, string> Invalid
        => new()
        {
            { "empty", "cover.png", "image/png", "Image file is empty." },
            { "too-large", "cover.png", "image/png", "Image must be smaller than 2 MB." },
            { "png", "cover.gif", "image/png", "Invalid image extension." },
            { "png", "cover.png", "image/gif", "Invalid image content type." },
            { "text", "cover.png", "image/png", "File content does not match an allowed image format." },
        };

    // The bytes for a kind named in Invalid.
    public static byte[] Bytes(string kind)
        => kind switch
        {
            "empty" => [],
            "too-large" => [.. Png, .. new byte[TooLargeBytes]],
            "png" => Png,
            "text" => "This is a text file, not an image."u8.ToArray(),
            _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null),
        };
}

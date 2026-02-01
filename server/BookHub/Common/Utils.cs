namespace BookHub.Common;

using System.Collections.Frozen;
using System.Text;
using Infrastructure.Services.Result;

using static Constants.DefaultValues;

public static class Utils
{
    private const int MaxImageSizeBytes = 2 * 1_024 * 1_024;

    private static readonly FrozenSet<string> AllowedExtensions =
        new[] { ".jpg", ".jpeg", ".png", ".webp", ".avif" }
            .ToFrozenSet(StringComparer.OrdinalIgnoreCase);

    private static readonly FrozenSet<string> AllowedContentTypes =
        new[] { "image/jpeg", "image/png", "image/webp", "image/avif" }
            .ToFrozenSet(StringComparer.OrdinalIgnoreCase);

    public static DateTime? StringToDateTime(string? dateTimeString)
    {
        if (string.IsNullOrEmpty(dateTimeString))
        {
            return null;
        }

        if (DateTime.TryParse(dateTimeString, out DateTime result))
        {
            return result;
        }

        return null;
    }

    public static string? DateTimeToString(DateTime? dateTime)
        => dateTime.HasValue ? dateTime.Value.ToString("O") : null;

    public static void ClampPageSizeAndIndex(
        ref int pageIndex,
        ref int pageSize,
        int maxPageIndex = DefaultPageIndex,
        int maxPageSize = DefaultPageSize)
    {
        pageIndex = Math.Min(pageIndex, maxPageIndex);
        pageSize = Math.Min(pageSize, maxPageSize);
    }

    public static Result ValidateImageFile(IFormFile image)
    {
        if (image.Length == 0)
        {
            return "Image file is empty.";
        }

        if (image.Length > MaxImageSizeBytes)
        {
            return $"Image must be smaller than {MaxImageSizeBytes / 1_024 / 1_024} MB.";
        }

        var extension = Path.GetExtension(image.FileName);
        if (!AllowedExtensions.Contains(extension))
        {
            return $"Invalid image extension. Allowed: {string.Join(", ", AllowedExtensions)}.";
        }

        if (!AllowedContentTypes.Contains(image.ContentType))
        {
            return $"Invalid image content type. Allowed: {string.Join(", ", AllowedContentTypes)}.";
        }

        if (!LooksLikeAllowedImage(image))
        {
            return "File content does not match an allowed image format.";
        }

        return true;
    }

    private static bool LooksLikeAllowedImage(IFormFile file)
    {
        Span<byte> header = stackalloc byte[16];

        using var stream = file.OpenReadStream();
        var read = stream.Read(header);

        const int MinHeaderBytes = 12;
        if (read < MinHeaderBytes)
        {
            return false;
        }

        var looksLikeJpeg = header[0] == 0xFF && header[1] == 0xD8 && header[2] == 0xFF;

        const int PngSignatureLength = 8;
        var hasPngHeader = read >= PngSignatureLength;

        var looksLikePng = hasPngHeader &&
            header[0] == 0x89 && header[1] == 0x50 && header[2] == 0x4E && header[3] == 0x47 &&
            header[4] == 0x0D && header[5] == 0x0A && header[6] == 0x1A && header[7] == 0x0A;

        var hasRiffHeader = read >= MinHeaderBytes;
        var looksLikeWebp = hasRiffHeader &&
            header[0] == (byte)'R' && header[1] == (byte)'I' && header[2] == (byte)'F' && header[3] == (byte)'F' &&
            header[8] == (byte)'W' && header[9] == (byte)'E' && header[10] == (byte)'B' && header[11] == (byte)'P';

        var looksLikeIsoBmff = hasRiffHeader &&
            header[4] == (byte)'f' && header[5] == (byte)'t' && header[6] == (byte)'y' && header[7] == (byte)'p';

        var looksLikeAvif = false;
        if (looksLikeIsoBmff)
        {
            var brand = Encoding.ASCII.GetString(header.Slice(8, 4));
            looksLikeAvif = brand is "avif" or "avis";
        }

        return looksLikeJpeg || looksLikePng || looksLikeWebp || looksLikeAvif;
    }
}

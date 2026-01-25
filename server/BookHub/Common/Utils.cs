namespace BookHub.Common;

using static Constants.DefaultValues;

public static class Utils
{
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
}

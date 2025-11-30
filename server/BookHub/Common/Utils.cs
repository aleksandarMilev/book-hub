namespace BookHub.Common
{
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
    }
}

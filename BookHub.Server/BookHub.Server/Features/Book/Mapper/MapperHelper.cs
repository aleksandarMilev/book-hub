namespace BookHub.Server.Features.Book.Mapper
{
    public static class MapperHelper
    {
        public static DateTime? ParseDateTime(string? dateTimeString)
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
    }

}

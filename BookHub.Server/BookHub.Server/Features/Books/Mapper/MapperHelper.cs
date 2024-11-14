namespace BookHub.Server.Features.Books.Mapper
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

namespace BookHub.Server.Features.Authors.Mapper
{
    using Data.Models.Enums;

    public static class MapperHelper
    {
        public static Gender ParseGender(string gender) 
            => Enum.TryParse(gender, true, out Gender result) ? result : Gender.Other;

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

namespace BookHub.Features.ReadingList.Service
{
    public class ReadingListTypeException(string readingListType)
        : Exception(string.Format(InvalidStatusType, readingListType))
    {
        public const string InvalidStatusType = "{0} is invalid reading list status type!";
    }
}

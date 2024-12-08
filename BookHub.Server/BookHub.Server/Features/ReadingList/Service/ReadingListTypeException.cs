namespace BookHub.Server.Features.ReadingList.Service
{
    public class ReadingListTypeException : Exception
    {
        public const string InvalidStatusType = "{0} is invalid reading list status type!";

        public ReadingListTypeException(string readingListType)
           : base(string.Format(InvalidStatusType, readingListType))
        {
        }
    }
}

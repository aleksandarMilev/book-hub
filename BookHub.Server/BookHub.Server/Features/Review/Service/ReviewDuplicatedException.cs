namespace BookHub.Server.Features.Review.Service
{
    public class ReviewDuplicatedException : Exception
    {
        private const string ErrorMessage = "{0} has already reviewed book with Id: {1}!";

        public ReviewDuplicatedException(string username, int bookId)
           : base(string.Format(ErrorMessage, username, bookId))
        {
        }
    }
}

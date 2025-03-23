namespace BookHub.Common
{
    public class DbEntityNotFoundException<TId> : Exception
    {
        private const string ErrorMessage = "{0} with Id: {1} was not found!";

        public DbEntityNotFoundException(string entityType, TId id)
           : base(string.Format(ErrorMessage, entityType, id)) { }
    }
}

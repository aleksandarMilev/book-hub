namespace BookHub.Common
{
    public class DbEntityNotFoundException<TId>(
        string entityType, TId id) : 
        Exception(string.Format(ErrorMessage, entityType, id))
    {
        private const string ErrorMessage = "{0} with Id: {1} was not found!";
    }
}

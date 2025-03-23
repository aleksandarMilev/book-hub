namespace BookHub.Infrastructure.Extensions
{
    using Data.Models.Base;

    public static class DbQueryExtensions
    {
        public static IQueryable<T> ApplyIsDeletedFilter<T>(this IQueryable<T> query) 
            where T : class, IDeletableEntity
            => query.Where(e => !e.IsDeleted);
    }
}

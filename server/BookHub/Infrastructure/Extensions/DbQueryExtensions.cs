namespace BookHub.Infrastructure.Extensions;

using Data.Models.Base;

public static class DbQueryExtensions
{
    extension<T>(IQueryable<T> query)
        where T : class, IDeletableEntity
    {
        public IQueryable<T> ApplyIsDeletedFilter()
            => query.Where(e => !e.IsDeleted);
    }
}

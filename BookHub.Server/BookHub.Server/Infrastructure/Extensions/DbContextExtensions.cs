namespace BookHub.Server.Infrastructure.Extensions
{
    using Data.Models.Base;
    using Microsoft.EntityFrameworkCore;

    public static class DbContextExtensions
    {
        public static IQueryable<T> ApplyIsDeletedFilter<T>(this IQueryable<T> dbSet) 
            where T : class, IDeletableEntity
                => dbSet.Where(e => !e.IsDeleted);
    }
}

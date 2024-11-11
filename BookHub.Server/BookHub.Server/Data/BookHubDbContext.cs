namespace BookHub.Server.Data
{
    using System.Linq.Expressions;

    using Infrastructure.Services;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Models.Base;
    using Seed;

    public class BookHubDbContext(
        DbContextOptions<BookHubDbContext> options, 
        ICurrentUserService userService) : IdentityDbContext<User>(options)
    {
        private readonly ICurrentUserService userService = userService;

        public DbSet<Book> Books { get; set; }

        public DbSet<Genre> Genres { get; set; }

        public DbSet<Author> Authors { get; set; }

        public DbSet<Nationality> Nationalities { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<Reply> Replies { get; set; }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.ApplyAuditInfo();

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            this.ApplyAuditInfo();

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<Nationality>()
                .HasData(NationalitiesSeeder.Seed());

            FilterDeletedModels(modelBuilder);
        }

        private void ApplyAuditInfo() 
            => this.ChangeTracker
                .Entries()
                .ToList()
                .ForEach(e =>
                {
                    var utcNow = DateTime.UtcNow;
                    var username = this.userService.GetUsername();

                    if (e.State == EntityState.Deleted && e.Entity is IDeletableEntity deletableEntity)
                    {
                        deletableEntity.DeletedOn = utcNow;
                        deletableEntity.DeletedBy = username;
                        deletableEntity.IsDeleted = true;

                        e.State = EntityState.Modified;

                        return;
                    }

                    if (e.Entity is IEntity entity)
                    {
                        if (e.State == EntityState.Added)
                        {
                            entity.CreatedOn = utcNow;
                            entity.CreatedBy = username!;
                        }
                        else if (e.State == EntityState.Modified)
                        {
                            entity.ModifiedOn = utcNow;
                            entity.ModifiedBy = username;
                        }
                    }
                });

        private static void FilterDeletedModels(ModelBuilder modelBuilder)
            => modelBuilder
                .Model
                .GetEntityTypes()
                .Where(e =>
                {
                    return typeof(IDeletableEntity).IsAssignableFrom(e.ClrType);
                })
                .ToList()
                .ForEach(e =>
                {
                    modelBuilder
                        .Entity(e.ClrType)
                        .HasQueryFilter(DeletableFilterExpression(e.ClrType));
                });

        private static LambdaExpression DeletableFilterExpression(Type entityType)
        {
            var parameter = Expression.Parameter(entityType, "e");
            var isDeletedProperty = Expression.Property(parameter, nameof(IDeletableEntity.IsDeleted));
            var isDeletedFalse = Expression.Equal(isDeletedProperty, Expression.Constant(false));

            return Expression.Lambda(isDeletedFalse, parameter);
        }
    }
}

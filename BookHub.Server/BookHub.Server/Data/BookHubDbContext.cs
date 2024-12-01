namespace BookHub.Server.Data
{
    using System.Linq.Expressions;
    using System.Reflection;

    using Infrastructure.Services;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Models.Base;

    public class BookHubDbContext(
        DbContextOptions<BookHubDbContext> options, 
        ICurrentUserService userService) : IdentityDbContext<User>(options)
    {
        private readonly ICurrentUserService userService = userService;

        public DbSet<Book> Books { get; init; }

        public DbSet<Genre> Genres { get; init; }

        public DbSet<BookGenre> BooksGenres { get; init; }

        public DbSet<ReadingList> ReadingLists { get; init; }

        public DbSet<Author> Authors { get; init; }

        public DbSet<Nationality> Nationalities { get; init; }

        public DbSet<Review> Reviews { get; init; }

        public DbSet<Vote> Votes { get; init; }

        public DbSet<Reply> Replies { get; init; }

        public DbSet<UserProfile> Profiles { get; init; }

        public DbSet<Article> Articles { get; init; }

        public DbSet<Notification> Notifications { get; init; }

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

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            FilterDeletedModels(modelBuilder);
            FilterUnapprovedModels(modelBuilder);
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

        private static void FilterUnapprovedModels(ModelBuilder modelBuilder)
              => modelBuilder
                  .Model
                  .GetEntityTypes()
                  .Where(e =>
                  {
                      return typeof(IApprovableEntity).IsAssignableFrom(e.ClrType);
                  })
                  .ToList()
                  .ForEach(e =>
                  {
                      modelBuilder
                          .Entity(e.ClrType)
                          .HasQueryFilter(ApprovableFilterExpression(e.ClrType));
                  });

        private static LambdaExpression DeletableFilterExpression(Type entityType)
        {
            var parameter = Expression.Parameter(entityType, "e");
            var isDeletedProperty = Expression.Property(parameter, nameof(IDeletableEntity.IsDeleted));
            var isDeletedFalse = Expression.Equal(isDeletedProperty, Expression.Constant(false));

            return Expression.Lambda(isDeletedFalse, parameter);
        }

        private static LambdaExpression ApprovableFilterExpression(Type entityType)
        {
            var parameter = Expression.Parameter(entityType, "e");
            var isApprovedProperty = Expression.Property(parameter, nameof(IApprovableEntity.IsApproved));
            var isApprovedFalse = Expression.Equal(isApprovedProperty, Expression.Constant(true));

            return Expression.Lambda(isApprovedFalse, parameter);
        }
    }
}

namespace BookHub.Server.Data
{
    using System.Linq.Expressions;

    using BookHub.Server.Data.Models;
    using BookHub.Server.Data.Models.Base;
    using BookHub.Server.Infrastructure.Services;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class BookHubDbContext : IdentityDbContext<User>
    {
        private readonly ICurrentUserService userService;

        public BookHubDbContext(DbContextOptions<BookHubDbContext> options, ICurrentUserService userService)
            : base(options)
            => this.userService = userService;

        public DbSet<Book> Books { get; set; }

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

            var deletableEntities = modelBuilder
                .Model
                .GetEntityTypes()
                .Where(e =>
                {
                    return typeof(IDeletableEntity).IsAssignableFrom(e.ClrType);
                });

            foreach (var e in deletableEntities)
            {
                modelBuilder
                    .Entity(e.ClrType)
                    .HasQueryFilter(MakeFilterExpression(e.ClrType));
            }
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

        private static LambdaExpression MakeFilterExpression(Type entityType)
        {
            var parameter = Expression.Parameter(entityType, "e");
            var isDeletedProperty = Expression.Property(parameter, nameof(IDeletableEntity.IsDeleted));
            var isDeletedFalse = Expression.Equal(isDeletedProperty, Expression.Constant(false));

            return Expression.Lambda(isDeletedFalse, parameter);
        }
    }
}

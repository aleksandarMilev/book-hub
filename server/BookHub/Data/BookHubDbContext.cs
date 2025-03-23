﻿namespace BookHub.Data
{
    using System.Linq.Expressions;
    using System.Reflection;
    using Features.Article.Data.Models;
    using Features.Authors.Data.Models;
    using Features.Book.Data.Models;
    using Features.Chat.Data.Models;
    using Features.Genre.Data.Models;
    using Features.Identity.Data.Models;
    using Features.Notification.Data.Models;
    using Features.ReadingList.Data.Models;
    using Features.Review.Data.Models;
    using Features.UserProfile.Data.Models;
    using Infrastructure.Services;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Models.Base;
    using Models.Shared.BookGenre;
    using Models.Shared.ChatUser;

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

        public DbSet<UserProfile> Profiles { get; init; }

        public DbSet<Article> Articles { get; init; }

        public DbSet<Notification> Notifications { get; init; }

        public DbSet<ChatMessage> ChatMessages { get; init; }

        public DbSet<Chat> Chats { get; init; }

        public DbSet<ChatUser> ChatsUsers { get; init; }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.ApplyAuditInfo();

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess, 
            CancellationToken cancellationToken = default)
        {
            this.ApplyAuditInfo();

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            FilterModels(modelBuilder);
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

        private static void FilterModels(ModelBuilder modelBuilder)
            => modelBuilder
                .Model
                .GetEntityTypes()
                .ToList()
                .ForEach(entityType =>
                {
                    var entityClrType = entityType.ClrType;
                    var filterExpression = BuildFilterExpression(entityClrType);

                    if (filterExpression != null)
                    {
                        modelBuilder.Entity(entityClrType).HasQueryFilter(filterExpression);
                    }
                });

        private static LambdaExpression? BuildFilterExpression(Type entityType)
        {
            var parameter = Expression.Parameter(entityType, "e");

            Expression? combinedFilter = null;

            if (typeof(IDeletableEntity).IsAssignableFrom(entityType))
            {
                var isDeletedProperty = Expression.Property(parameter, nameof(IDeletableEntity.IsDeleted));
                var isNotDeleted = Expression.Equal(isDeletedProperty, Expression.Constant(false));
                combinedFilter = isNotDeleted;
            }

            if (typeof(IApprovableEntity).IsAssignableFrom(entityType))
            {
                var isApprovedProperty = Expression.Property(parameter, nameof(IApprovableEntity.IsApproved));
                var isApproved = Expression.Equal(isApprovedProperty, Expression.Constant(true));

                combinedFilter = combinedFilter == null
                    ? isApproved
                    : Expression.AndAlso(combinedFilter, isApproved);
            }

            return combinedFilter == null
                ? null
                : Expression.Lambda(combinedFilter, parameter);
        }
    }
}

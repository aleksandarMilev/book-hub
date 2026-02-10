namespace BookHub.Data;

using System.Linq.Expressions;
using System.Reflection;
using Features.Articles.Data.Models;
using Features.Authors.Data.Models;
using Features.Books.Data.Models;
using Features.Chat.Data.Models;
using Features.Genres.Data.Models;
using Features.Identity.Data.Models;
using Features.Notifications.Data.Models;
using Features.ReadingLists.Data.Models;
using Features.Reviews.Data.Models;
using Features.UserProfile.Data.Models;
using Infrastructure.Services.CurrentUser;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models.Base;
using Models.Shared.BookGenre.Models;
using Models.Shared.ChatUser;

public class BookHubDbContext(
    DbContextOptions<BookHubDbContext> options, 
    ICurrentUserService userService) : IdentityDbContext<UserDbModel>(options)
{
    public DbSet<BookDbModel> Books { get; init; }

    public DbSet<GenreDbModel> Genres { get; init; }

    public DbSet<BookGenreDbModel> BooksGenres { get; init; }

    public DbSet<ReadingListDbModel> ReadingLists { get; init; }

    public DbSet<AuthorDbModel> Authors { get; init; }

    public DbSet<ReviewDbModel> Reviews { get; init; }

    public DbSet<VoteDbModel> Votes { get; init; }

    public DbSet<UserProfile> Profiles { get; init; }

    public DbSet<ArticleDbModel> Articles { get; init; }

    public DbSet<NotificationDbModel> Notifications { get; init; }

    public DbSet<ChatMessageDbModel> ChatMessages { get; init; }

    public DbSet<ChatDbModel> Chats { get; init; }

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

        return base.SaveChangesAsync(
            acceptAllChangesOnSuccess,
            cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            Assembly.GetExecutingAssembly());

        FilterModels(modelBuilder);
    }

    private void ApplyAuditInfo() 
        => this.ChangeTracker
            .Entries()
            .ToList()
            .ForEach(entry =>
            {
                var utcNow = DateTime.UtcNow;
                var username = userService.GetUsername();

                if (entry.State == EntityState.Deleted && 
                    entry.Entity is IDeletableEntity deletableEntity)
                {
                    deletableEntity.DeletedOn = utcNow;
                    deletableEntity.DeletedBy = username;
                    deletableEntity.IsDeleted = true;

                    entry.State = EntityState.Modified;

                    return;
                }

                if (entry.Entity is IDeletableEntity entity)
                {
                    if (entry.State == EntityState.Added)
                    {
                        entity.CreatedOn = utcNow;
                        entity.CreatedBy = username!;
                    }
                    else if (entry.State == EntityState.Modified)
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

                if (filterExpression is not null)
                {
                    modelBuilder
                    .Entity(entityClrType)
                    .HasQueryFilter(filterExpression);
                }
            });

    private static LambdaExpression? BuildFilterExpression(Type entityType)
    {
        var parameter = Expression.Parameter(entityType, "e");
        Expression? combinedFilter = null;

        if (typeof(IDeletableEntity).IsAssignableFrom(entityType))
        {
            var isDeletedProperty = Expression.Property(
                parameter,
                nameof(IDeletableEntity.IsDeleted));

            var isNotDeleted = Expression.Equal(
                isDeletedProperty,
                Expression.Constant(false));

            combinedFilter = isNotDeleted;
        }

        if (typeof(IApprovableEntity).IsAssignableFrom(entityType))
        {
            var isApprovedProperty = Expression.Property(
                parameter,
                nameof(IApprovableEntity.IsApproved));

            var isApproved = Expression.Equal(
                isApprovedProperty,
                Expression.Constant(true));

            combinedFilter = combinedFilter is null
                ? isApproved
                : Expression.AndAlso(combinedFilter, isApproved);
        }

        return combinedFilter is null
            ? null
            : Expression.Lambda(combinedFilter, parameter);
    }
}

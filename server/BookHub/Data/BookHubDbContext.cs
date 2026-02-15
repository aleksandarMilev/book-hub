namespace BookHub.Data;

using System.Linq.Expressions;
using System.Reflection;
using BookHub.Features.Challenges.Data.Models;
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
    public string? CurrentUserId { get; } = userService.GetId();

    public bool IsAdmin { get; } = userService.IsAdmin();

    public DbSet<BookDbModel> Books { get; init; }

    public DbSet<BookEditDbModel> BookEdits { get; init; }

    public DbSet<GenreDbModel> Genres { get; init; }

    public DbSet<BookGenreDbModel> BooksGenres { get; init; }

    public DbSet<ReadingListDbModel> ReadingLists { get; init; }

    public DbSet<AuthorDbModel> Authors { get; init; }

    public DbSet<AuthorEditDbModel> AuthorEdits { get; init; }

    public DbSet<ReviewDbModel> Reviews { get; init; }

    public DbSet<VoteDbModel> Votes { get; init; }

    public DbSet<UserProfile> Profiles { get; init; }

    public DbSet<ArticleDbModel> Articles { get; init; }

    public DbSet<NotificationDbModel> Notifications { get; init; }

    public DbSet<ChatMessageDbModel> ChatMessages { get; init; }

    public DbSet<ChatDbModel> Chats { get; init; }

    public DbSet<ChatUser> ChatsUsers { get; init; }

    public DbSet<ReadingChallengeDbModel> ReadingChallenges { get; init; }

    public DbSet<ReadingCheckInDbModel> ReadingCheckIns { get; init; }

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

        this.FilterModels(modelBuilder);
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

    private void FilterModels(ModelBuilder modelBuilder)
        => modelBuilder
            .Model
            .GetEntityTypes()
            .ToList()
            .ForEach(entityType =>
            {
                var clrType = entityType.ClrType;
                var filter = BuildFilterExpression(clrType);

                if (filter is not null)
                {
                    modelBuilder.Entity(clrType).HasQueryFilter(filter);
                }
            });

    private LambdaExpression? BuildFilterExpression(Type entityType)
    {
        var entityTypeParam = Expression.Parameter(entityType, "e");
        Expression? combined = null;

        if (typeof(IDeletableEntity).IsAssignableFrom(entityType))
        {
            var isDeleted = Expression.Property(
                entityTypeParam,
                nameof(IDeletableEntity.IsDeleted));

            var isNotDeleted = Expression.Equal(
                isDeleted,
                Expression.Constant(false));

            combined = isNotDeleted;
        }

        var thisContext = Expression.Constant(this);

        if (typeof(IApprovableEntity).IsAssignableFrom(entityType))
        {
            var isApprovedProp = Expression.Property(
                entityTypeParam,
                nameof(IApprovableEntity.IsApproved));

            var isApproved = Expression.Equal(
                isApprovedProp,
                Expression.Constant(true));

            var isAdmin = Expression.Property(
                thisContext,
                nameof(this.IsAdmin));

            var isAdminTrue = Expression.Equal(
                isAdmin,
                Expression.Constant(true));

            Expression creatorOr = Expression.OrElse(
                isApproved,
                isAdminTrue);

            var creatorIdProp = entityType.GetProperty("CreatorId");
            if (creatorIdProp is not null &&
                creatorIdProp.PropertyType == typeof(string))
            {
                var creatorId = Expression.Property(
                    entityTypeParam,
                    creatorIdProp);

                var currentUserId = Expression.Property(
                    thisContext,
                    nameof(CurrentUserId));

                var currentUserNotNull = Expression.NotEqual(
                    currentUserId,
                    Expression.Constant(null, typeof(string)));

                var isCreator = Expression.Equal(
                    creatorId,
                    currentUserId);

                var creatorAllowed = Expression.AndAlso(
                    currentUserNotNull,
                    isCreator);

                creatorOr = Expression.OrElse(
                    creatorOr,
                    creatorAllowed);
            }

            combined = combined is null
                ? creatorOr
                : Expression.AndAlso(combined, creatorOr);
        }

        return combined is null
            ? null
            : Expression.Lambda(combined, entityTypeParam);
    }
}

namespace BookHub.Features.Search.Shared;

using Article.Data.Models;
using Authors.Data.Models;
using Book.Data.Models;
using Features.Chat.Data.Models;
using Genre.Data.Models;
using Genre.Service.Models;
using Service.Models;
using UserProfile.Data.Models;

public static class SearchMapping
{
    public static IQueryable<SearchGenreServiceModel> ToSearchSeviceModels(
        this IQueryable<GenreDbModel> genres)
        => genres.Select(g => new SearchGenreServiceModel
        {
            Id = g.Id,
            Name = g.Name,
            ImagePath = g.ImagePath
        });

    public static IQueryable<SearchBookServiceModel> ToSearchSeviceModels(
       this IQueryable<BookDbModel> books)
       => books.Select(b => new SearchBookServiceModel
       {
           Id = b.Id,
           Title = b.Title,
           AuthorName = b.Author == null ? null : b.Author.Name,
           ShortDescription = b.ShortDescription,
           ImagePath = b.ImagePath,
           AverageRating = b.AverageRating,
           Genres = b
            .BooksGenres
            .Select(bg => new GenreNameServiceModel
            {
                Id = bg.GenreId,
                Name = bg.Genre.Name
            })
            .ToHashSet(),
       });

    public static IQueryable<SearchAuthorServiceModel> ToSearchSeviceModels(
        this IQueryable<AuthorDbModel> authors)
        => authors.Select(a => new SearchAuthorServiceModel
        {
            Id = a.Id,
            Name = a.Name,
            PenName = a.PenName,
            AverageRating = a.AverageRating,
            ImagePath = a.ImagePath,
        });

    public static IQueryable<SearchArticleServiceModel> ToSearchSeviceModels(
        this IQueryable<ArticleDbModel> artciles)
        => artciles.Select(a => new SearchArticleServiceModel
        {
            Id = a.Id,
            Title = a.Title,
            Introduction = a.Introduction,
            Views = a.Views,
            ImagePath = a.ImagePath,
            CreatedOn = a.CreatedOn,
        });

    public static IQueryable<SearchProfileServiceModel> ToSearchSeviceModels(
        this IQueryable<UserProfile> profiles)
        => profiles.Select(p => new SearchProfileServiceModel
        {
            Id = p.UserId,
            FirstName = p.FirstName,
            LastName = p.LastName,
            ImageUrl = p.ImageUrl,
            IsPrivate = p.IsPrivate,
        });

    public static IQueryable<SearchChatServiceModel> ToSearchSeviceModels(
        this IQueryable<Chat> profiles)
        => profiles.Select(p => new SearchChatServiceModel
        {
            Id = p.Id,
            Name = p.Name,
            ImageUrl = p.ImageUrl,
            CreatorId = p.CreatorId,
        });
}

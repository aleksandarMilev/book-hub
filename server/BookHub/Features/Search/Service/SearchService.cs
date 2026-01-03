namespace BookHub.Features.Search.Service;

using BookHub.Common;
using Data;
using Infrastructure.Services.CurrentUser;
using Microsoft.EntityFrameworkCore;
using Models;
using Shared;

using static Common.Constants.DefaultValues;

public class SearchService(
    BookHubDbContext data,
    ICurrentUserService userService) : ISearchService
{
    public async Task<PaginatedModel<SearchGenreServiceModel>> Genres(
        string? searchTerm,
        int pageIndex,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        ClampPageSizeAndIndex(
            ref pageIndex,
            ref pageSize);

        var genres = data
            .Genres
            .AsNoTracking()
            .ToSearchSeviceModels();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            genres = genres
                .Where(g => g.
                    Name
                    .ToLower()
                    .Contains(searchTerm.ToLower()));
        }

        genres = genres
            .OrderByDescending(b => b.Name);

        var total = await genres.CountAsync(cancellationToken);
        var items = await genres
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedModel<SearchGenreServiceModel>(
            items,
            total,
            pageIndex,
            pageSize);
    }

    public async Task<PaginatedModel<SearchBookServiceModel>> Books(
        string? searchTerm,
        int pageIndex,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        ClampPageSizeAndIndex(
            ref pageIndex,
            ref pageSize);

        var books = data
            .Books
            .AsNoTracking()
            .ToSearchSeviceModels();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            books = books.Where(b =>
                b.Title.ToLower().Contains(searchTerm.ToLower()) ||
                b.ShortDescription.ToLower().Contains(searchTerm.ToLower()) ||
                b.AuthorName != null && b.AuthorName.ToLower().Contains(searchTerm.ToLower())
            );
        }

        books = books
            .OrderByDescending(b => b.AverageRating);

        var total = await books.CountAsync(cancellationToken);
        var items = await books
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedModel<SearchBookServiceModel>(
            items,
            total,
            pageIndex,
            pageSize);
    }

    public async Task<PaginatedModel<SearchArticleServiceModel>> Articles(
        string? searchTerm,
        int pageIndex,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        ClampPageSizeAndIndex(
            ref pageIndex,
            ref pageSize);

        var articles = data
            .Articles
            .AsNoTracking()
            .ToSearchSeviceModels();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            articles = articles.Where(a =>
                a.Title.ToLower().Contains(searchTerm.ToLower()) ||
                a.Introduction.ToLower().Contains(searchTerm.ToLower())
            );
        }

        articles = articles
            .OrderByDescending(a => a.Views)
            .ThenByDescending(b => b.CreatedOn);

        var total = await articles.CountAsync(cancellationToken);
        var items = await articles
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedModel<SearchArticleServiceModel>(
            items,
            total,
            pageIndex,
            pageSize);
    }

    public async Task<PaginatedModel<SearchAuthorServiceModel>> Authors(
        string? searchTerm,
        int pageIndex,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        ClampPageSizeAndIndex(
            ref pageIndex,
            ref pageSize);

        var authors = data
            .Authors
            .AsNoTracking()
            .ToSearchSeviceModels();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            authors = authors
                .Where(a =>
                    a.Name.ToLower().Contains(searchTerm.ToLower()) ||
                    (a.PenName != null && a.PenName.ToLower().Contains(searchTerm.ToLower()))
            );
        }

        authors = authors
            .OrderByDescending(b => b.AverageRating);

        var total = await authors.CountAsync(cancellationToken);
        var items = await authors
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedModel<SearchAuthorServiceModel>(
            items,
            total,
            pageIndex,
            pageSize);
    }

    public async Task<PaginatedModel<SearchProfileServiceModel>> Profiles(
        string? searchTerm,
        int pageIndex,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        ClampPageSizeAndIndex(
            ref pageIndex,
            ref pageSize);

        var profiles = data.Profiles.ToSearchSeviceModels();
        var term = searchTerm?.Trim();

        if (!string.IsNullOrEmpty(term))
        {
            var safe = term.Replace("\"", "\"\"");
            var fullTextQuery = $"\"{safe}*\"";

            profiles = profiles
                .Where(p =>
                    EF.Functions.Contains(p.FirstName, fullTextQuery) ||
                    EF.Functions.Contains(p.LastName, fullTextQuery));
        }

        profiles = profiles
            .OrderBy(p => p.LastName)
            .ThenBy(p => p.FirstName)
            .ThenBy(p => p.Id);

        var total = await profiles.CountAsync(cancellationToken);
        var items = await profiles
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedModel<SearchProfileServiceModel>(
            items,
            total,
            pageIndex,
            pageSize);
    }

    public async Task<PaginatedModel<SearchChatServiceModel>> Chats(
        string? searchTerm,
        int pageIndex,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        ClampPageSizeAndIndex(
            ref pageIndex,
            ref pageSize);

        var chats = data
             .Chats
             .AsNoTracking();

        if (userService.IsAdmin())
        {
            chats = chats
                .Where(c => c
                    .ChatsUsers
                    .Any(cu =>
                        cu.UserId == userService.GetId() &&
                        cu.HasAccepted));
        }

        var chatModels = chats.ToSearchSeviceModels();
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            chatModels = chatModels
                .Where(c => c
                    .Name
                    .ToLower()
                    .Contains(searchTerm.ToLower()));
        }

        var total = await chatModels.CountAsync(cancellationToken);
        var items = await chatModels
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedModel<SearchChatServiceModel>(
            items,
            total,
            pageIndex,
            pageSize);
    }

    private static void ClampPageSizeAndIndex(
        ref int pageIndex,
        ref int pageSize)
    {
        pageIndex = pageIndex < DefaultPageIndex ? DefaultPageIndex : pageIndex;
        pageSize = pageSize < DefaultPageIndex ? DefaultPageSize : pageSize;
    }
}

namespace BookHub.Features.Search.Service;

using BookHub.Common;
using Data;
using Infrastructure.Services.CurrentUser;
using Microsoft.EntityFrameworkCore;
using Models;
using Shared;

using static Common.Utils;

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

        var term = searchTerm?.Trim();

        if (!string.IsNullOrEmpty(term))
        {
            var safe = term.Replace("\"", "\"\"");
            var fullTextQuery = $"\"{safe}*\"";

            genres = genres
                .Where(g => EF.Functions.Contains(g.Name, fullTextQuery));
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

        var dbModels = data
            .Books
            .AsNoTracking();

        var term = searchTerm?.Trim();
        if (!string.IsNullOrEmpty(term))
        {
            var safe = term.Replace("\"", "\"\"");
            var fullTextQuery = $"\"{safe}*\"";

            dbModels = dbModels.Where(b =>
                EF.Functions.Contains(b.Title, fullTextQuery) ||
                EF.Functions.Contains(b.ShortDescription, fullTextQuery) ||
                (b.Author != null && EF.Functions.Contains(b.Author.Name, fullTextQuery)) ||
                (b.Author != null && b.Author.PenName != null && EF.Functions.Contains(b.Author.PenName, fullTextQuery)));
        }

        var books = dbModels
            .OrderByDescending(b => b.AverageRating)
            .ToSearchSeviceModels();

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

        var term = searchTerm?.Trim();
        if (!string.IsNullOrEmpty(term))
        {
            var safe = term.Replace("\"", "\"\"");
            var fullTextQuery = $"\"{safe}*\"";

            articles = articles.Where(a =>
                EF.Functions.Contains(a.Title, fullTextQuery) ||
                EF.Functions.Contains(a.Introduction, fullTextQuery));
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

        var term = searchTerm?.Trim();

        if (!string.IsNullOrEmpty(term))
        {
            var safe = term.Replace("\"", "\"\"");
            var fullTextQuery = $"\"{safe}*\"";

            authors = authors.Where(a =>
                EF.Functions.Contains(a.Name, fullTextQuery) ||
                (a.PenName != null && EF.Functions.Contains(a.PenName, fullTextQuery)));
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

        if (!userService.IsAdmin())
        {
            chats = chats
                .Where(c => c
                    .ChatsUsers
                    .Any(cu =>
                        cu.UserId == userService.GetId() &&
                        cu.HasAccepted));
        }

        var chatModels = chats.ToSearchSeviceModels();
        var term = searchTerm?.Trim();

        if (!string.IsNullOrEmpty(term))
        {
            var safe = term.Replace("\"", "\"\"");
            var fullTextQuery = $"\"{safe}*\"";

            chatModels = chatModels
                .Where(c => EF.Functions.Contains(c.Name, fullTextQuery));
        }

        chatModels = chatModels.OrderBy(c => c.Name);

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
}

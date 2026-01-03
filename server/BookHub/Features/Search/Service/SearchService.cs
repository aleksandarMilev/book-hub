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
        int page,
        int pageSize,
        CancellationToken token = default)
    {
        var genres = data
            .Genres
            .ToSearchSeviceModels();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            genres = genres
                .Where(g => g.Name.ToLower().Contains(searchTerm.ToLower()));
        }

        genres = genres.OrderByDescending(b => b.Name);

        var totalGenres = await genres.CountAsync(token);
        var paginatedGenres = await genres
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(token);

        return new PaginatedModel<SearchGenreServiceModel>(
            paginatedGenres,
            totalGenres,
            page,
            pageSize);
    }

    public async Task<PaginatedModel<SearchBookServiceModel>> Books(
        string? searchTerm,
        int page,
        int pageSize,
        CancellationToken token = default)
    {
        var books = data
            .Books
            .ToSearchSeviceModels();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            books = books.Where(b =>
                b.Title.ToLower().Contains(searchTerm.ToLower()) ||
                b.ShortDescription.ToLower().Contains(searchTerm.ToLower()) ||
                b.AuthorName != null && b.AuthorName.ToLower().Contains(searchTerm.ToLower())
            );
        }

        books = books.OrderByDescending(b => b.AverageRating);

        var totalBooks = await books.CountAsync(token);
        var paginatedBooks = await books
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(token);

        return new PaginatedModel<SearchBookServiceModel>(
            paginatedBooks,
            totalBooks,
            page,
            pageSize);
    }

    public async Task<PaginatedModel<SearchArticleServiceModel>> Articles(
        string? searchTerm,
        int pageIndex,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var articles = data
            .Articles
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
        int page,
        int pageSize,
        CancellationToken token = default)
    {
        var authors = data
            .Authors
            .ToSearchSeviceModels();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            authors = authors.Where(a =>
                a.Name.ToLower().Contains(searchTerm.ToLower()) ||
               (a.PenName != null && a.PenName.ToLower().Contains(searchTerm.ToLower()))
            );
        }

        authors = authors.OrderByDescending(b => b.AverageRating);

        var total = await authors.CountAsync(token);
        var paginatedAuthors = await authors
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(token);

        return new PaginatedModel<SearchAuthorServiceModel>(
            paginatedAuthors,
            total,
            page,
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
        int page,
        int pageSize,
        CancellationToken token = default)
    {
        var chats = data
             .Chats
             .AsQueryable();

        if (userService.IsAdmin())
        {
            chats = chats.Where(c => c.ChatsUsers.Any(cu =>
                cu.UserId == userService.GetId() &&
                cu.HasAccepted
             ));
        }

        var chatModels = chats.ToSearchSeviceModels();
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            chatModels = chatModels.Where(c => c.Name.ToLower().Contains(searchTerm.ToLower()));
        }

        var total = await chatModels.CountAsync(token);
        var paginatedChats = await chatModels
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(token);

        return new PaginatedModel<SearchChatServiceModel>(
            paginatedChats,
            total,
            page,
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

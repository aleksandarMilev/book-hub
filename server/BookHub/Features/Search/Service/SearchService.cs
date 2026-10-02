namespace BookHub.Features.Search.Service;

using Common;
using Data;
using Infrastructure.Extensions;
using Infrastructure.Services.PageClamper;
using Microsoft.EntityFrameworkCore;
using Models;
using Shared;

public class SearchService(
    BookHubDbContext data,
    IPageClamper pageClamper) : ISearchService
{
    public async Task<PaginatedModel<SearchGenreServiceModel>> Genres(
        string? searchTerm,
        int pageIndex,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        pageClamper.ClampPageSizeAndIndex(
            ref pageIndex,
            ref pageSize);

        var genres = data
            .Genres
            .AsNoTracking()
            .ApplyFullTextSearch(searchTerm)
            .ToSearchSeviceModels()
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
        pageClamper.ClampPageSizeAndIndex(
            ref pageIndex,
            ref pageSize);

        var books = data
            .Books
            .AsNoTracking()
            .ApplyFullTextSearch(searchTerm)
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
        pageClamper.ClampPageSizeAndIndex(
            ref pageIndex,
            ref pageSize);

        var articles = data
            .Articles
            .AsNoTracking()
            .ApplyFullTextSearch(searchTerm)
            .ToSearchSeviceModels()
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
        pageClamper.ClampPageSizeAndIndex(
            ref pageIndex,
            ref pageSize);

        var authors = data
            .Authors
            .AsNoTracking()
            .ApplyFullTextSearch(searchTerm)
            .ToSearchSeviceModels()
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
        pageClamper.ClampPageSizeAndIndex(
            ref pageIndex,
            ref pageSize);

        var profiles = data
            .Profiles
            .ApplyFullTextSearch(searchTerm)
            .ToSearchSeviceModels()
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
}

namespace BookHub.Features.Search.Service
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using BookHub.Infrastructure.Services.CurrentUser;
    using Data;
    using Infrastructure.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class SearchService(
        BookHubDbContext data,
        ICurrentUserService userService,
        IMapper mapper) : ISearchService
    {
        private readonly BookHubDbContext data = data;
        private readonly ICurrentUserService userService = userService;
        private readonly IMapper mapper = mapper;

        public async Task<PaginatedModel<SearchBookServiceModel>> Books(
            string? searchTerm,
            int page,
            int pageSize)
        {
            var books = this.data
                .Books
                .ProjectTo<SearchBookServiceModel>(this.mapper.ConfigurationProvider);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                books = books.Where(b =>
                    b.Title.ToLower().Contains(searchTerm.ToLower()) ||
                    b.ShortDescription.ToLower().Contains(searchTerm.ToLower()) ||
                    b.AuthorName != null && b.AuthorName.ToLower().Contains(searchTerm.ToLower())
                );
            }

            books = books.OrderByDescending(b => b.AverageRating);

            var totalBooks = await books.CountAsync();
            var paginatedBooks = await books
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedModel<SearchBookServiceModel>(
                paginatedBooks,
                totalBooks,
                page,
                pageSize);
        }

        [AllowAnonymous]
        public async Task<PaginatedModel<SearchArticleServiceModel>> Articles(
            string? searchTerm,
            int page,
            int pageSize)
        {
            var articles = this.data
                .Articles
                .ProjectTo<SearchArticleServiceModel>(this.mapper.ConfigurationProvider);

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

            var totalArticles = await articles.CountAsync();
            var paginatedArticles = await articles
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedModel<SearchArticleServiceModel>(paginatedArticles, totalArticles, page, pageSize);
        }

        public async Task<PaginatedModel<SearchAuthorServiceModel>> Authors(
            string? searchTerm,
            int page,
            int pageSize)
        {
            var authors = this.data
                .Authors
                .ProjectTo<SearchAuthorServiceModel>(this.mapper.ConfigurationProvider);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                authors = authors.Where(a =>
                    a.Name.ToLower().Contains(searchTerm.ToLower()) ||
                   (a.PenName != null && a.PenName.ToLower().Contains(searchTerm.ToLower()))
                );
            }

            authors = authors.OrderByDescending(b => b.AverageRating);

            var total = await authors.CountAsync();
            var paginatedAuthors = await authors
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedModel<SearchAuthorServiceModel>(
                paginatedAuthors,
                total,
                page,
                pageSize);
        }

        public async Task<PaginatedModel<SearchProfileServiceModel>> Profiles(
            string? searchTerm,
            int page,
            int pageSize)
        {
            var profiles = this.data
                 .Profiles
                 .ProjectTo<SearchProfileServiceModel>(this.mapper.ConfigurationProvider);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                profiles = profiles.Where(a =>
                    a.FirstName.ToLower().Contains(searchTerm.ToLower()) ||
                    a.LastName.ToLower().Contains(searchTerm.ToLower()) ||
                    (a.FirstName.ToLower() + " " + a.LastName.ToLower()).Contains(searchTerm.ToLower())
                );
            }

            var total = await profiles.CountAsync();
            var paginatedProfiles = await profiles
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedModel<SearchProfileServiceModel>(
                paginatedProfiles,
                total,
                page,
                pageSize);
        }

        public async Task<PaginatedModel<SearchChatServiceModel>> Chats(
            string? searchTerm,
            int page,
            int pageSize)
        {
            var chats = this.data
                 .Chats
                 .AsQueryable();

            if (!this.userService.IsAdmin())
            {
                chats = chats.Where(c => c.ChatsUsers.Any(cu =>
                    cu.UserId == this.userService.GetId() &&
                    cu.HasAccepted
                 ));
            }

             var chatModels = chats.ProjectTo<SearchChatServiceModel>(this.mapper.ConfigurationProvider);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                chatModels = chatModels.Where(c => c.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            var total = await chatModels.CountAsync();
            var paginatedChats = await chatModels
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedModel<SearchChatServiceModel>(
                paginatedChats,
                total,
                page,
                pageSize);
        }
    }
}

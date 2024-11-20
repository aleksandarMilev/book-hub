namespace BookHub.Server.Features.Search.Service
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class SearchService(
        BookHubDbContext data,
        IMapper mapper) : ISearchService
    {
        private readonly BookHubDbContext data = data;
        private readonly IMapper mapper = mapper;

        public async Task<PaginatedModel<SearchBookServiceModel>> GetBooksAsync(string? searchTerm, int page, int pageSize)
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

            var totalBooks = await books.CountAsync();

            var paginatedBooks = await books
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedModel<SearchBookServiceModel>(paginatedBooks, totalBooks, page, pageSize);
        }
    }
}

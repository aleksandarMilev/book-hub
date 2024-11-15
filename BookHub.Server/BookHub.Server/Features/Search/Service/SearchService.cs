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

        public async Task<IEnumerable<SearchBookServiceModel>> GetBooksAsync(string? searchTerm)
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

            return await books.ToListAsync();
        }
    }
}

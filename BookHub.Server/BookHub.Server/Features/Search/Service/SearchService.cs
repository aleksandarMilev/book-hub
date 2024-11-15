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

        public async Task<IEnumerable<SearchBookServiceModel>> GetBooksAsync(string searchTerm)
            => await this.data
                .Books
                .Where(b => 
                    b.Title.ToLower().Contains(searchTerm.ToLower()) ||
                    b.ShortDescription.ToLower().Contains(searchTerm.ToLower()
                ))
                .ProjectTo<SearchBookServiceModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();
    }
}

namespace BookHub.Features.Genre.Service
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using BookHub.Data;

    public class GenreService(
        BookHubDbContext data,
        IMapper mapper) : IGenreService
    {
        private readonly BookHubDbContext data = data;
        private readonly IMapper mapper = mapper;

        public async Task<IEnumerable<GenreNameServiceModel>> Names()
          => await this.data
              .Genres
              .ProjectTo<GenreNameServiceModel>(this.mapper.ConfigurationProvider)
              .ToListAsync();

        public async Task<GenreDetailsServiceModel?> Details(int id)
           => await this.data
               .Genres
               .ProjectTo<GenreDetailsServiceModel>(this.mapper.ConfigurationProvider)
               .FirstOrDefaultAsync(g => g.Id == id);
    }
}

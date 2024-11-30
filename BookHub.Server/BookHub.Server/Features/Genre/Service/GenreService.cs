namespace BookHub.Server.Features.Genre.Service
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class GenreService(
        BookHubDbContext data,
        IMapper mapper) : IGenreService
    {
        private readonly BookHubDbContext data = data;
        private readonly IMapper mapper = mapper;

        public async Task<IEnumerable<GenreNameServiceModel>> NamesAsync()
          => await data
              .Genres
              .ProjectTo<GenreNameServiceModel>(mapper.ConfigurationProvider)
              .ToListAsync();

        public async Task<GenreDetailsServiceModel?> DetailsAsync(int id)
           => await data
               .Genres
               .ProjectTo<GenreDetailsServiceModel>(mapper.ConfigurationProvider)
               .FirstOrDefaultAsync(g => g.Id == id);
    }
}

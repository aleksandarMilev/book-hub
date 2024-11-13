namespace BookHub.Server.Features.Authors.Service
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using Data.Models;
    using Infrastructure.Services;
    using Microsoft.EntityFrameworkCore;
    using Models;

    using static Common.Constants.DefaultValues;
    using static Common.Messages.Error.Author;

    public class AuthorService(
        BookHubDbContext data,
        IMapper mapper) : IAuthorService
    {
        private readonly BookHubDbContext data = data;
        private readonly IMapper mapper = mapper;

        public IEnumerable<string> GetNationalities()
           => this.data
               .Nationalities
               .Select(n => n.Name)
               .ToList();

        public async Task<IEnumerable<AuthorServiceModel>> GetTopThreeAsync()
            => await this.data
                .Authors
                .ProjectTo<AuthorServiceModel>(this.mapper.ConfigurationProvider)
                .OrderByDescending(a => a.Rating)
                .Take(3)
                .ToListAsync();

        public async Task<AuthorDetailsServiceModel?> GetDetailsAsync(int id)
            => await this.data
                .Authors
                .ProjectTo<AuthorDetailsServiceModel>(this.mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(a => a.Id == id);

        public async Task<int> CreateAsync(CreateAuthorServiceModel model)
        {
            var author = this.mapper.Map<Author>(model);
            author.NationalityId = await GetNationalityByNameAsync(model.Nationality);
            author.ImageUrl ??= DefaultAuthorImageUrl;

            this.data.Add(author);
            await this.data.SaveChangesAsync();

            return author.Id;
        }

        public async Task<Result> EditAsync(int id, CreateAuthorServiceModel model)
        {
            var author = await this.data
                 .Authors
                 .FindAsync(id);

            if (author is null)
            {
                return AuthorNotFound;
            }

            if (author.CreatorId != model.CreatorId)
            {
                return UnauthorizedAuthorEdit;
            }

            this.mapper.Map(model, author);
            await this.data.SaveChangesAsync();

            return true;
        }

        public async Task<Result> DeleteAsync(int id, string userId)
        {
            var author = await this.data
                 .Authors
                 .FindAsync(id);

            if (author is null)
            {
                return AuthorNotFound;
            }

            if (author.CreatorId != userId)
            {
                return UnauthorizedAuthorDelete;
            }

            this.data.Remove(author);
            await this.data.SaveChangesAsync();

            return true;
        }

        private async Task<int> GetNationalityByNameAsync(string name)
        {
            int? nationalityId = await this.data
                .Nationalities
                .Where(n => n.Name == name)
                .Select(n => n.Id)
                .FirstOrDefaultAsync();

            nationalityId ??= await this.data
                .Nationalities
                .Where(n => n.Name == UnknownNationalityName)
                .Select(n => n.Id)
                .FirstOrDefaultAsync();

            return nationalityId.Value;
        }
    }
}

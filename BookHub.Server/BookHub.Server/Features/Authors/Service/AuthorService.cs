namespace BookHub.Server.Features.Authors.Service
{
    using AutoMapper;
    using Data;
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Models;

    using static Common.Constants.DefaultValues;

    public class AuthorService(
        BookHubDbContext data,
        IMapper mapper) : IAuthorService
    {
        private readonly BookHubDbContext data = data;
        private readonly IMapper mapper = mapper;

        public List<string> GetNationalities()
            => this.data
                .Nationalities
                .Select(n => n.Name)
                .ToList();

        public async Task<int> CreateAsync(AuthorDetailsServiceModel model)
        {
            var author = this.mapper.Map<Author>(model);
            author.NationalityId = await GetNationalityByNameAsync(model);

            this.data.Add(author);
            await this.data.SaveChangesAsync();

            return author.Id;
        }

        private async Task<int> GetNationalityByNameAsync(AuthorDetailsServiceModel model)
        {
            int? nationalityId = await this.data
                .Nationalities
                .Where(n => n.Name == model.Nationality)
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

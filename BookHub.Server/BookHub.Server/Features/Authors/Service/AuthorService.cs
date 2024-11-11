namespace BookHub.Server.Features.Authors.Service
{
    using AutoMapper;
    using Data;
    using Data.Models;
    using Models;

    public class AuthorService(
        BookHubDbContext data,
        IMapper mapper) : IAuthorService
    {
        private readonly BookHubDbContext data = data;
        private readonly IMapper mapper = mapper;

        public async Task<int> CreateAsync(AuthorDetailsServiceModel model)
        {
            var author = this.mapper.Map<Author>(model);

            this.data.Add(author);
            await this.data.SaveChangesAsync();

            return author.Id;
        }
    }
}

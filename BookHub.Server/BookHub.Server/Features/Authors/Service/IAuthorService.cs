namespace BookHub.Server.Features.Authors.Service
{
    using Models;

    public interface IAuthorService
    {
        List<string> GetNationalities();

        Task<int> CreateAsync(AuthorDetailsServiceModel model);
    }
}

namespace BookHub.Server.Features.Authors.Service
{
    using Models;

    public interface IAuthorService
    {
        Task<int> CreateAsync(AuthorDetailsServiceModel model);
    }
}

namespace BookHub.Server.Features.Authors.Service
{
    public class AuthorService : IAuthorService
    {
        public async Task<int> Create()
        {
            await Task.Delay(1_000);
            return 1;
        }
    }
}

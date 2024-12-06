namespace BookHub.Server.Features.Chat.Service
{
    using Infrastructure.Services;
    using Infrastructure.Services.ServiceLifetimes;
    using Service.Models;

    public interface IChatService : ITransientService
    {
        Task<IEnumerable<ChatServiceModel>> AllAsync();

        Task<ChatDetailsServiceModel?> DetailsAsync(int id);

        Task<int> CreateAsync(CreateChatServiceModel model);

        Task<Result> EditAsync(int id);

        Task<Result> DeleteAsync(int id);
    }
}

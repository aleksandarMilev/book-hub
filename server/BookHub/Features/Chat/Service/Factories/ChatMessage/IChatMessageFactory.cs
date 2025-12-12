
namespace BookHub.Features.Chat.Service.Factories.ChatMessage;

using Infrastructure.Services.ServiceLifetimes;
using Models;

public interface IChatMessageFactory : ITransientService
{
    IChatMessageFactory WithId(int id);

    IChatMessageFactory WithMessage(string message);

    IChatMessageFactory WithSenderId(string senderId);

    IChatMessageFactory WithSenderName(string senderName);

    IChatMessageFactory WithSenderImagePath(string senderImagePath);

    IChatMessageFactory CreatedOn(DateTime CreatedOn);

    ChatMessageServiceModel Build();
}

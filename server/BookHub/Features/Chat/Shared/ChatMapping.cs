namespace BookHub.Features.Chat.Shared;

using Data.Models;
using Service.Models;
using UserProfile.Data.Models;
using Web.Models;

public static class ChatMapping
{
    public static IQueryable<ChatMessageProfileServiceModel> ToChatMessageProfileServiceModel(
        this IQueryable<UserProfile> profiles)
        => profiles.Select(p => new ChatMessageProfileServiceModel
        {
            Name = p.FirstName + " " + p.LastName,
            ImagePath = p.ImagePath,
        });

    public static CreateChatMessageServiceModel ToCreateChatMessageServiceModel(
        this CreateChatMessageWebModel webModel)
        => new()
        {
            Message = webModel.Message,
            ChatId = webModel.ChatId,
        };

    public static ChatMessageDbModel ToChatMessageDbModel(
        this CreateChatMessageServiceModel serviceModel)
        => new()
        {
            Message = serviceModel.Message,
            ChatId = serviceModel.ChatId,
        };

    public static void UpdateChatMessageDbModel(
        this CreateChatMessageServiceModel serviceModel,
        ChatMessageDbModel dbModel)
        => dbModel.Message = serviceModel.Message;

    public static CreateChatServiceModel ToCreateChatServiceModel(
        this CreateChatWebModel webModel)
        => new()
        {
            Name = webModel.Name,
            Image = webModel.Image,
        };

    public static ProcessChatInvitationServiceModel ToProcessChatInvitationServiceModel(
        this ProcessChatInvitationWebModel webModel)
        => new()
        {
            ChatId = webModel.ChatId,
            ChatCreatorId = webModel.ChatCreatorId,
            ChatName = webModel.ChatName,
        };

    public static AddUserToChatServiceModel ToAddUserToChatWebModel(
        this AddUserToChatWebModel webModel)
        => new()
        {
            UserId = webModel.UserId,
            ChatName = webModel.ChatName,
        };
}

namespace BookHub.Features.Chat.Shared;

using Data.Models;
using Service.Models;
using UserProfile.Data.Models;
using UserProfile.Service.Models;
using Web.Models;

public static class ChatMapping
{
    public static IQueryable<PrivateProfileServiceModel> ToPrivateProfileServiceModel(
        this IQueryable<UserProfile> profiles)
        => profiles.Select(p => new PrivateProfileServiceModel
        {
            Id = p.UserId,
            FirstName = p.FirstName,
            LastName = p.LastName,
            ImagePath = p.ImagePath,
            IsPrivate = p.IsPrivate,
        });

    public static ChatDbModel ToDbModel(
        this CreateChatServiceModel serviceModel)
        => new()
        {
            Name = serviceModel.Name,
        };

    public static void UpdateDbModel(
        this CreateChatServiceModel serviceModel,
        ChatDbModel dbModel)
    {
        dbModel.Name = serviceModel.Name;
    }

    public static ChatDetailsServiceModel ToChatDetailsServiceModels(
        this ChatDbModel dbModel)
        => new()
        {
            Id = dbModel.Id,
            Name = dbModel.Name,
            ImagePath = dbModel.ImagePath,
            CreatorId = dbModel.CreatorId,
            Participants = dbModel
                .ChatsUsers
                .Where(cu => cu.HasAccepted)
                .Select(cu => new PrivateProfileServiceModel
                {
                    Id = cu.UserId,
                    FirstName = cu.User.Profile!.FirstName,
                    LastName = cu.User.Profile.LastName,
                    ImagePath = cu.User.Profile.ImagePath,
                    IsPrivate = cu.User.Profile.IsPrivate
                })
                .ToHashSet()
        };

    public static IQueryable<ChatDetailsServiceModel> ToChatDetailsServiceModels(
        this IQueryable<ChatDbModel> chats)
        => chats.Select(c => new ChatDetailsServiceModel
        {
            Id = c.Id,
            Name = c.Name,
            ImagePath = c.ImagePath,
            CreatorId = c.CreatorId,
            Participants = c
                .ChatsUsers
                .Where(cu => cu.HasAccepted)
                .Select(cu => new PrivateProfileServiceModel
                {
                    Id = cu.UserId,
                    FirstName = cu.User.Profile!.FirstName,
                    LastName = cu.User.Profile.LastName,
                    ImagePath = cu.User.Profile.ImagePath,
                    IsPrivate = cu.User.Profile.IsPrivate
                })
                .ToHashSet()
        });

    public static IQueryable<ChatServiceModel> ToChatServiceModels(
        this IQueryable<ChatDbModel> chats)
        => chats.Select(c => new ChatServiceModel
        {
            Id = c.Id,
            Name = c.Name,
            ImagePath = c.ImagePath,
            CreatorId = c.CreatorId,
        });

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

    public static IQueryable<ChatMessageServiceModel> ToServiceModels(
        this IQueryable<ChatMessageDbModel> dbModels)
        => dbModels.Select(m => new ChatMessageServiceModel
        {
            Id = m.Id,
            Message = m.Message,
            SenderId = m.SenderId,
            SenderName = m.Sender.Profile!.FirstName + " " + m.Sender.Profile.LastName,
            SenderImagePath = m.Sender.Profile.ImagePath,
            CreatedOn = m.CreatedOn,
            ModifiedOn = m.ModifiedOn
        });
}

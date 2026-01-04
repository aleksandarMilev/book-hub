namespace BookHub.Features.Chat.Service;

using BookHub.Data;
using BookHub.Data.Models.Shared.ChatUser;
using Data.Models;
using Infrastructure.Services.CurrentUser;
using Infrastructure.Services.ImageWriter;
using Infrastructure.Services.Result;
using Microsoft.EntityFrameworkCore;
using Notifications.Service;
using Service.Models;
using Shared;
using UserProfile.Data.Models;
using UserProfile.Service.Models;

using static Common.Constants.ErrorMessages;
using static Shared.Constants.Paths;

public class ChatService(
    BookHubDbContext data,
    ICurrentUserService userService,
    INotificationService notificationService,
    IImageWriter imageWriter,
    ILogger<ChatService> logger) : IChatService
{
    public async Task<ChatDetailsServiceModel?> Details(
        Guid chatId,
        CancellationToken token = default)
    {
        var userId = userService.GetId()!;

        var canAccessChat = await this.CanAccessChat(chatId, userId, token);
        if (!canAccessChat)
        {
            return null;
        }

        var chat = await data
            .Chats
            .ToChatDetailsServiceModels()
            .FirstOrDefaultAsync(c => c.Id == chatId, token);

        if (chat is null)
        {
            return null;
        }

        var messages = await data
            .ChatMessages
            .Where(m => m.ChatId == chatId)
            .ToServiceModels()
            .OrderByDescending(m => m.Id)
            .Take(20)
            .ToListAsync(token);

        messages.Reverse();
        chat.Messages = messages;

        return chat;
    }

    public async Task<IEnumerable<ChatServiceModel>> NotJoined(
        string userToJoinId,
        CancellationToken token = default)
        => await data
            .Chats
            .Where(c => 
                c.CreatorId == userService.GetId() && 
                !c.ChatsUsers.Any(cu => cu.UserId == userToJoinId))
            .ToChatServiceModels()
            .ToListAsync(token);

    public async Task<bool> CanAccessChat(
        Guid chatId,
        string userId,
        CancellationToken token = default)
        => await data
            .Chats
            .AnyAsync(
                c =>
                    c.Id == chatId &&
                    c.ChatsUsers.Any(cu => cu.UserId == userId && cu.HasAccepted),
                token);

    public async Task<bool> IsInvited(
            Guid chatId,
            string userId,
            CancellationToken token = default)
            => await data
                .Chats
                .AnyAsync(
                    c =>
                        c.Id == chatId &&
                        c.ChatsUsers.Any(cu => cu.UserId == userId && !cu.HasAccepted),
                    token);

    public async Task<ResultWith<ChatDetailsServiceModel>> Create(
        CreateChatServiceModel serviceModel,
        CancellationToken token = default)
    {
        var creatorId = userService.GetId()!;

        var dbModel = serviceModel.ToDbModel();
        dbModel.CreatorId = creatorId;

        await imageWriter.Write(
           ImagePathPrefix,
           dbModel,
           serviceModel,
           DefaultImagePath,
           token);

        data.Add(dbModel);

        var chatUserEntityCreationResult = await this.CreateChatUserEntity(
            dbModel.Id,
            creatorId,
            true,
            token);

        if (!chatUserEntityCreationResult.Succeeded)
        {
            return chatUserEntityCreationResult.ErrorMessage!;
        }

        await data.SaveChangesAsync(token);

        logger.LogInformation(
            "New chat with Id: {id} was created.",
            dbModel.Id);

        var serviceModelResult = dbModel.ToChatDetailsServiceModels();
        return ResultWith<ChatDetailsServiceModel>.Success(serviceModelResult);
    }

    public async Task<Result> Edit(
        Guid chatId,
        CreateChatServiceModel serviceModel,
        CancellationToken token = default)
    {
        var dbModel = await this.GetChatDbModel(chatId, token);
        if (dbModel is null)
        {
            return this.LogAndReturnNotFoundMessage(
                nameof(ChatDbModel),
                chatId);
        }

        var userId = userService.GetId()!;
        if (dbModel.CreatorId != userId)
        {
            return LogAndReturnUnauthorizedMessage(
                userId,
                nameof(ChatDbModel),
                chatId);
        }

        var oldImagePath = dbModel.ImagePath;
        var isNewImageUploaded = serviceModel.Image is not null;

        serviceModel.UpdateDbModel(dbModel);

        await imageWriter.Write(
            ImagePathPrefix,
            dbModel,
            serviceModel,
            null,
            token);

        if (isNewImageUploaded)
        {
            imageWriter.Delete(
                nameof(ChatDbModel),
                oldImagePath,
                DefaultImagePath);
        }

        await data.SaveChangesAsync(token);

        logger.LogInformation(
            "Chat with Id: {id} was updated.",
            dbModel.Id);

        return true;
    }

    public async Task<Result> Delete(
        Guid chatId,
        CancellationToken token = default)
    {
        var dbModel = await this.GetChatDbModel(chatId, token);
        if (dbModel is null)
        {
            return LogAndReturnNotFoundMessage(
                nameof(ChatDbModel),
                chatId);
        }

        var userId = userService.GetId()!;
        var isNotCreator = dbModel.CreatorId != userId;
        var isNotAdmin = !userService.IsAdmin();

        if (isNotCreator && isNotAdmin)
        {
            return LogAndReturnUnauthorizedMessage(
                userId,
                nameof(ChatDbModel),
                chatId);
        }

        data.Remove(dbModel);
        await data.SaveChangesAsync(token);

        logger.LogInformation(
            "Chat with Id: {id} was deleted.",
            dbModel.Id);

        return true;
    }

    public async Task<ResultWith<PrivateProfileServiceModel>> Accept(
        ProcessChatInvitationServiceModel serviceModel,
        CancellationToken token = default)
    {
        var chatId = serviceModel.ChatId;
        var invitedUserId = userService.GetId()!;
        var mapEntity = await this.GetChatUserDbModel(
            invitedUserId,
            chatId,
            token);

        if (mapEntity is null)
        {
            return this.LogAndReturnNotFoundMessage(
                nameof(ChatUser),
                $"{chatId}-{invitedUserId}");
        }

        mapEntity.HasAccepted = true;
        await data.SaveChangesAsync(token);

        logger.LogInformation(
            "User with Id: {userId} accepted to in chat with Id: {chatId}",
            invitedUserId,
            chatId);

        await notificationService
            .CreateOnChatInvitationAccepted(
                chatId,
                serviceModel.ChatName,
                serviceModel.ChatCreatorId,
                token);

        var profile = await data
            .Profiles
            .Where(p => p.UserId == invitedUserId)
            .ToPrivateProfileServiceModel()
            .FirstOrDefaultAsync(token);

        if (profile is null)
        {
            return this.LogAndReturnNotFoundMessage(
                nameof(UserProfile),
                invitedUserId);
        }

        return ResultWith<PrivateProfileServiceModel>.Success(profile);
    }

    public async Task<Result> Reject(
        ProcessChatInvitationServiceModel serviceModel,
        CancellationToken token = default)
    {
        var chatId = serviceModel.ChatId;
        var invitedUserId = userService.GetId()!;
        var mapEntity = await this.GetChatUserDbModel(
            invitedUserId,
            chatId,
            token);

        if (mapEntity is null)
        {
            return this.LogAndReturnNotFoundMessage(
                nameof(ChatUser),
                $"{chatId}-{invitedUserId}");
        }

        data.Remove(mapEntity);
        await data.SaveChangesAsync(token);

        logger.LogInformation(
            "User with Id: {userId} rejected to in chat with Id: {chatId}",
            invitedUserId,
            chatId);

        await notificationService
            .CreateOnChatInvitationRejected(
                chatId,
                serviceModel.ChatName,
                serviceModel.ChatCreatorId,
                token);

        return true;
    }

    public async Task<Result> InviteUserToChat(
       Guid chatId,
       AddUserToChatServiceModel serviceModel,
       CancellationToken token = default)
    {
        var userToInviteId = serviceModel.UserId;
        var currentUserId = userService.GetId()!;
        var chatCreatorId = await data
           .Chats
           .Where(c => c.Id == chatId)
           .Select(c => c.CreatorId)
           .FirstOrDefaultAsync(token);

        if (chatCreatorId != currentUserId)
        {
            return this.LogAndReturnUnauthorizedMessage(
                currentUserId,
                nameof(ChatDbModel),
                chatId);
        }

        var chatUserEntityCreationResult = await this.CreateChatUserEntity(
            chatId,
            userToInviteId,
            false,
            token);

        if (!chatUserEntityCreationResult.Succeeded)
        {
            return chatUserEntityCreationResult.ErrorMessage!;
        }

        await data.SaveChangesAsync(token);

        logger.LogInformation(
            "User with Id: {userToInviteId} was invited to join in chat with Id: {chatId}",
            userToInviteId,
            chatId);

        await notificationService.CreateOnChatInvitation(
            chatId,
            serviceModel.ChatName,
            userToInviteId,
            token);

        return true;
    }

    public async Task<Result> RemoveUserFromChat(
        Guid chatId,
        string userToRemoveId,
        CancellationToken token = default)
    {
        var currentUserId = userService.GetId()!;
        var chatCreatorId = await data
            .Chats
            .Where(c => c.Id == chatId)
            .Select(c => c.CreatorId)
            .FirstOrDefaultAsync(token);

        var isNotCreator = chatCreatorId != currentUserId;
        var notToBeRemoved = userToRemoveId != currentUserId;

        if (isNotCreator && notToBeRemoved)
        {
            return this.LogAndReturnUnauthorizedMessage(
                currentUserId,
                nameof(ChatDbModel),
                chatId);
        }

        var mapEntity = await this.GetChatUserDbModel(
            userToRemoveId,
            chatId,
            token);

        if (mapEntity is null)
        {
            return this.LogAndReturnNotFoundMessage(
                nameof(ChatDbModel),
                $"{chatId}-{userToRemoveId}");
        }

        data.Remove(mapEntity);
        await data.SaveChangesAsync(token);

        logger.LogInformation(
            "User with Id: {userToRemoveId} was removed from chat with Id: {chatId}",
            userToRemoveId,
            chatId);

        return true;
    }

    private async Task<Result> CreateChatUserEntity(
        Guid chatId,
        string userId,
        bool hasAccepted,
        CancellationToken cancellationToken = default)
    {
        var alreadyInvited = await data
            .ChatsUsers
            .AsNoTracking()
            .AnyAsync(
                cu => cu.ChatId == chatId && cu.UserId == userId,
                cancellationToken);

        if (alreadyInvited)
        {
            return $"User with Id: {userId} is already invited to or participant in Chat with Id: {chatId}";
        }

        var chatUser = new ChatUser
        {
            UserId = userId,
            ChatId = chatId,
            HasAccepted = hasAccepted
        };

        data.Add(chatUser);

        return true;
    }

    private async Task<ChatDbModel?> GetChatDbModel(
        Guid id,
        CancellationToken token = default)
        => await data
            .Chats
            .FindAsync([id], token);

    private async Task<ChatUser?> GetChatUserDbModel(
        string userId,
        Guid chatId,
        CancellationToken token = default)
        => await data
            .ChatsUsers
            .FirstOrDefaultAsync(
                cu => cu.UserId == userId && cu.ChatId == chatId,
                token);

    private string LogAndReturnNotFoundMessage<TId>(
        string entityName,
        TId id)
    {
        logger.LogWarning(
            DbEntityNotFoundTemplate,
            entityName,
            id);

        return string.Format(
            DbEntityNotFound,
            entityName,
            id);
    }

    private string LogAndReturnUnauthorizedMessage<TId>(
        string userId,
        string resourceName,
        TId resourceId)
    {
        logger.LogWarning(
            UnauthorizedMessageTemplate,
            userId,
            resourceName,
            resourceId);

        return string.Format(
            UnauthorizedMessage,
            userId,
            resourceName,
            resourceId);
    }
}

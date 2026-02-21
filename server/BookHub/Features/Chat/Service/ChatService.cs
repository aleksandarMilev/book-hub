namespace BookHub.Features.Chat.Service;

using BookHub.Data;
using BookHub.Data.Models.Shared.ChatUser;
using Data.Models;
using Infrastructure.Services.CurrentUser;
using Infrastructure.Services.ImageWriter;
using Infrastructure.Services.Result;
using Infrastructure.Services.StringSanitizer;
using Microsoft.EntityFrameworkCore;
using Models;
using Notifications.Service;
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
    IStringSanitizerService stringSanitizer,
    ILogger<ChatService> logger) : IChatService
{
    public async Task<ChatDetailsServiceModel?> Details(
        Guid chatId,
        CancellationToken cancelationToken = default)
    {
        var userId = userService.GetId()!;
        var canAccessChat = await this.CanAccessChat(
            chatId,
            userId,
            cancelationToken);

        if (!canAccessChat)
        {
            return null;
        }

        var chat = await data
            .Chats
            .AsNoTracking()
            .ToChatDetailsServiceModels()
            .FirstOrDefaultAsync(
                c => c.Id == chatId,
                cancelationToken);

        if (chat is null)
        {
            return null;
        }

        var messages = await data
            .ChatMessages
            .AsNoTracking()
            .Where(m => m.ChatId == chatId)
            .ToServiceModels()
            .OrderByDescending(m => m.Id)
            .Take(20)
            .ToListAsync(cancelationToken);

        messages.Reverse();
        chat.Messages = messages;

        return chat;
    }

    public async Task<IEnumerable<ChatServiceModel>> NotJoined(
        string userToJoinId,
        CancellationToken cancelationToken = default)
        => await data
            .Chats
            .AsNoTracking()
            .Where(c => 
                c.CreatorId == userService.GetId() && 
                !c.ChatsUsers.Any(cu => cu.UserId == userToJoinId))
            .ToChatServiceModels()
            .ToListAsync(cancelationToken);

    public async Task<bool> CanAccessChat(
        Guid chatId,
        string userId,
        CancellationToken cancelationToken = default)
        => await data
            .Chats
            .AsNoTracking()
            .AnyAsync(
                c =>
                    c.Id == chatId &&
                    c.ChatsUsers.Any(cu => cu.UserId == userId),
                cancelationToken);

    public async Task<bool> CanAccessChatAndHasAcceptedInvitation(
        Guid chatId,
        string userId,
        CancellationToken cancelationToken = default)
        => await data
            .Chats
            .AsNoTracking()
            .AnyAsync(
                c =>
                    c.Id == chatId &&
                    c.ChatsUsers.Any(cu => cu.UserId == userId && cu.HasAccepted),
                cancelationToken);

    public async Task<bool> IsInvited(
            Guid chatId,
            string userId,
            CancellationToken cancelationToken = default)
            => await data
                .Chats
                .AsNoTracking()
                .AnyAsync(
                    c =>
                        c.Id == chatId &&
                        c.ChatsUsers.Any(cu => cu.UserId == userId && !cu.HasAccepted),
                    cancelationToken);

    public async Task<ResultWith<ChatDetailsServiceModel>> Create(
        CreateChatServiceModel serviceModel,
        CancellationToken cancelationToken = default)
    {
        var creatorId = userService.GetId()!;

        var dbModel = serviceModel.ToDbModel();
        dbModel.CreatorId = creatorId;

        await imageWriter.Write(
           ImagePathPrefix,
           dbModel,
           serviceModel,
           DefaultImagePath,
           cancelationToken);

        data.Add(dbModel);

        var chatUserEntityCreationResult = await this.CreateChatUserEntity(
            dbModel.Id,
            creatorId,
            true,
            cancelationToken);

        if (!chatUserEntityCreationResult.Succeeded)
        {
            return chatUserEntityCreationResult.ErrorMessage!;
        }

        await data.SaveChangesAsync(cancelationToken);

        logger.LogInformation(
            "New chat with Id: {id} was created.",
            dbModel.Id);

        var serviceModelResult = await data
            .Chats
            .AsNoTracking()
            .ToChatDetailsServiceModels()
            .FirstOrDefaultAsync(
                c => c.Id == dbModel.Id,
                cancelationToken);

        return ResultWith<ChatDetailsServiceModel>.Success(serviceModelResult!);
    }

    public async Task<Result> Edit(
        Guid chatId,
        CreateChatServiceModel serviceModel,
        CancellationToken cancelationToken = default)
    {
        var dbModel = await this.GetChatDbModel(
            chatId,
            cancelationToken);

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
            cancelationToken);

        if (isNewImageUploaded)
        {
            imageWriter.Delete(
                nameof(ChatDbModel),
                oldImagePath,
                DefaultImagePath);
        }

        await data.SaveChangesAsync(cancelationToken);

        logger.LogInformation(
            "Chat with Id: {id} was updated.",
            dbModel.Id);

        return true;
    }

    public async Task<Result> Delete(
        Guid chatId,
        CancellationToken cancelationToken = default)
    {
        var dbModel = await this.GetChatDbModel(
            chatId,
            cancelationToken);

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
        await data.SaveChangesAsync(cancelationToken);

        logger.LogInformation(
            "Chat with Id: {id} was deleted.",
            dbModel.Id);

        return true;
    }

    public async Task<ResultWith<PrivateProfileServiceModel>> Accept(
        ProcessChatInvitationServiceModel serviceModel,
        CancellationToken cancelationToken = default)
    {
        var chatId = serviceModel.ChatId;
        var invitedUserId = userService.GetId()!;
        var mapEntity = await this.GetChatUserDbModel(
            invitedUserId,
            chatId,
            cancelationToken);

        if (mapEntity is null)
        {
            return this.LogAndReturnNotFoundMessage(
                nameof(ChatUser),
                $"{chatId}-{invitedUserId}");
        }

        mapEntity.HasAccepted = true;
        await data.SaveChangesAsync(cancelationToken);

        logger.LogInformation(
            "User with Id: {userId} accepted to in chat with Id: {chatId}",
            invitedUserId,
            chatId);

        await notificationService
            .CreateOnChatInvitationAccepted(
                chatId,
                serviceModel.ChatName,
                serviceModel.ChatCreatorId,
                cancelationToken);

        var profile = await data
            .Profiles
            .Where(p => p.UserId == invitedUserId)
            .ToPrivateProfileServiceModel()
            .FirstOrDefaultAsync(cancelationToken);

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
        CancellationToken cancelationToken = default)
    {
        var chatId = serviceModel.ChatId;
        var invitedUserId = userService.GetId()!;
        var mapEntity = await this.GetChatUserDbModel(
            invitedUserId,
            chatId,
            cancelationToken);

        if (mapEntity is null)
        {
            return this.LogAndReturnNotFoundMessage(
                nameof(ChatUser),
                $"{chatId}-{invitedUserId}");
        }

        data.Remove(mapEntity);
        await data.SaveChangesAsync(cancelationToken);

        logger.LogInformation(
            "User with Id: {userId} rejected to in chat with Id: {chatId}",
            invitedUserId,
            chatId);

        await notificationService
            .CreateOnChatInvitationRejected(
                chatId,
                serviceModel.ChatName,
                serviceModel.ChatCreatorId,
                cancelationToken);

        return true;
    }

    public async Task<Result> InviteUserToChat(
       Guid chatId,
       AddUserToChatServiceModel serviceModel,
       CancellationToken cancelationToken = default)
    {
        var userToInviteId = serviceModel.UserId;
        var currentUserId = userService.GetId()!;
        var chatCreatorId = await data
           .Chats
           .AsNoTracking()
           .Where(c => c.Id == chatId)
           .Select(c => c.CreatorId)
           .FirstOrDefaultAsync(cancelationToken);

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
            cancelationToken);

        if (!chatUserEntityCreationResult.Succeeded)
        {
            return chatUserEntityCreationResult.ErrorMessage!;
        }

        await data.SaveChangesAsync(cancelationToken);

        logger.LogInformation(
            "User with Id: {userToInviteId} was invited to join in chat with Id: {chatId}",
            stringSanitizer.SanitizeStringForLog(userToInviteId),
            chatId);

        await notificationService.CreateOnChatInvitation(
            chatId,
            serviceModel.ChatName,
            userToInviteId,
            cancelationToken);

        return true;
    }

    public async Task<Result> RemoveUserFromChat(
        Guid chatId,
        string userToRemoveId,
        CancellationToken cancelationToken = default)
    {
        var currentUserId = userService.GetId()!;
        var chatCreatorId = await data
            .Chats
            .AsNoTracking()
            .Where(c => c.Id == chatId)
            .Select(c => c.CreatorId)
            .FirstOrDefaultAsync(cancelationToken);

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
            cancelationToken);

        if (mapEntity is null)
        {
            return this.LogAndReturnNotFoundMessage(
                nameof(ChatDbModel),
                $"{chatId}-{userToRemoveId}");
        }

        data.Remove(mapEntity);
        await data.SaveChangesAsync(cancelationToken);

        logger.LogInformation(
            "User with Id: {userToRemoveId} was removed from chat with Id: {chatId}",
            stringSanitizer.SanitizeStringForLog(userToRemoveId),
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
        CancellationToken cancelationToken = default)
        => await data
            .Chats
            .FindAsync([id], cancelationToken);

    private async Task<ChatUser?> GetChatUserDbModel(
        string userId,
        Guid chatId,
        CancellationToken cancelationToken = default)
        => await data
            .ChatsUsers
            .FirstOrDefaultAsync(
                cu => cu.UserId == userId && cu.ChatId == chatId,
                cancelationToken);

    private string LogAndReturnNotFoundMessage<TId>(
        string entityName,
        TId id)
    {
        var sanitizedId = id is string str
            ? stringSanitizer.SanitizeStringForLog(str)
            : (object?)id;

        logger.LogWarning(
            DbEntityNotFoundTemplate,
            entityName,
            sanitizedId);

        return string.Format(
            DbEntityNotFound,
            entityName,
            sanitizedId);
    }

    private string LogAndReturnUnauthorizedMessage<TId>(
        string userId,
        string resourceName,
        TId resourceId)
    {
        var sanitizedUserId = stringSanitizer.SanitizeStringForLog(userId);
        var sanitizedResourceId = resourceId is string str
            ? stringSanitizer.SanitizeStringForLog(str)
            : (object?)resourceId;

        logger.LogWarning(
            UnauthorizedMessageTemplate,
            sanitizedUserId,
            resourceName,
            sanitizedResourceId);

        return string.Format(
            UnauthorizedMessage,
            sanitizedUserId,
            resourceName,
            sanitizedResourceId);
    }
}

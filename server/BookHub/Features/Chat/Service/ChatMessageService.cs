namespace BookHub.Features.Chat.Service;

using BookHub.Data;
using Data.Models;
using Infrastructure.Services.CurrentUser;
using Infrastructure.Services.Result;
using Microsoft.EntityFrameworkCore;
using Service.Models;
using Shared;
using UserProfile.Data.Models;

using static Common.Constants.ErrorMessages;

public class ChatMessageService(
    BookHubDbContext data,
    ICurrentUserService userService,
    IChatService chatService,
    ILogger<ChatMessageService> logger) : IChatMessageService
{
    public async Task<ResultWith<IEnumerable<ChatMessageServiceModel>>> GetForChat(
        Guid chatId,
        int? before = null,
        int take = 50,
        CancellationToken token = default)
    {
        var userId = userService.GetId()!;

        var canAccessChat = await chatService.CanAccessChat(chatId, userId, token);
        if (!canAccessChat)
        {
            return this.LogAndReturnUnauthorizedMessage(
                userId,
                nameof(ChatDbModel),
                chatId);
        }

        take = Math.Clamp(take, 1, 100);

        var query = data
            .ChatMessages
            .Where(m => m.ChatId == chatId);

        if (before.HasValue)
        {
            query = query.Where(m => m.Id < before.Value);
        }

        var items = await query
            .ToServiceModels()
            .OrderByDescending(m => m.Id)
            .Take(take)
            .ToListAsync(token);

        items.Reverse();

        return ResultWith<IEnumerable<ChatMessageServiceModel>>.Success(items);
    }

    public async Task<ResultWith<ChatMessageServiceModel>> Create(
        CreateChatMessageServiceModel serviceModel,
        CancellationToken token = default)
    {
        var userId = userService.GetId()!;
        var chatId = serviceModel.ChatId;

        var canAccessChat = await chatService.CanAccessChat(
            chatId,
            userId,
            token);

        if (!canAccessChat)
        {
            return this.LogAndReturnUnauthorizedMessage(
                userId,
                nameof(ChatDbModel),
                chatId);
        }

        var dbModel = serviceModel.ToChatMessageDbModel();
        dbModel.SenderId = userId;

        data.Add(dbModel);
        await data.SaveChangesAsync(token);

        var result = await this.GetServiceModelById(dbModel.Id, token);
        if (result is null)
        {
            return this.LogAndReturnProfileNotFoundMessage(userId);
        }

        return ResultWith<ChatMessageServiceModel>.Success(result);
    }

    public async Task<ResultWith<ChatMessageServiceModel>> Edit(
        int chatMessageId, 
        CreateChatMessageServiceModel serviceModel,
        CancellationToken token = default)
    {
        var userId = userService.GetId()!;
        
        var dbModel = await this.GetDbModel(chatMessageId, token);
        if (dbModel is null)
        {
            return this.LogAndReturnChatMessageNotFoundMessage(chatMessageId);
        }

        if (dbModel.SenderId != userId)
        {
            return this.LogAndReturnUnauthorizedMessage(
                userId,
                nameof(ChatMessageDbModel),
                chatMessageId);
        }

        var chatId = dbModel.ChatId;
        var canAccessChat = await chatService.CanAccessChat(
            chatId,
            userId,
            token);

        if (!canAccessChat)
        {
            return this.LogAndReturnUnauthorizedMessage(
                userId,
                nameof(ChatDbModel),
                chatId);
        }

        serviceModel.UpdateChatMessageDbModel(dbModel);
        await data.SaveChangesAsync(token);

        var result = await this.GetServiceModelById(dbModel.Id, token);
        if (result is null)
        {
            return this.LogAndReturnProfileNotFoundMessage(userId);
        }

        return ResultWith<ChatMessageServiceModel>.Success(result);
    }

    public async Task<Result> Delete(
        int chatMessageId,
        CancellationToken token = default)
    {
        var userId = userService.GetId()!;

        var dbModel = await this.GetDbModel(chatMessageId, token);
        if (dbModel is null)
        {
            return this.LogAndReturnChatMessageNotFoundMessage(chatMessageId);
        }

        var chatId = dbModel.ChatId;
        var canAccessChat = await chatService.CanAccessChat(
            chatId,
            userId,
            token);

        if (!canAccessChat)
        {
            return this.LogAndReturnUnauthorizedMessage(
                userId,
                nameof(ChatDbModel),
                chatId);
        }

        if (dbModel.SenderId != userId)
        {
            return this.LogAndReturnUnauthorizedMessage(
                userId,
                nameof(ChatMessageDbModel),
                chatMessageId);
        }

        data.Remove(dbModel);
        await data.SaveChangesAsync(token);

        return true;
    }

    private async Task<ChatMessageDbModel?> GetDbModel(
        int id,
        CancellationToken token = default)
        => await data
            .ChatMessages
            .FindAsync([id], token);

    private string LogAndReturnProfileNotFoundMessage(string id)
    {
        logger.LogWarning(
            DbEntityNotFoundTemplate,
            nameof(UserProfile),
            id);

        return string.Format(
            DbEntityNotFound,
            nameof(UserProfile),
            id);
    }

    private string LogAndReturnChatMessageNotFoundMessage(int id)
    {
        logger.LogWarning(
            DbEntityNotFoundTemplate,
            nameof(ChatMessageDbModel),
            id);

        return string.Format(
            DbEntityNotFound,
            nameof(ChatMessageDbModel),
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

    private Task<ChatMessageServiceModel?> GetServiceModelById(
        int id,
        CancellationToken token)
        => data.ChatMessages
            .Where(m => m.Id == id)
            .ToServiceModels()
            .FirstOrDefaultAsync(token);
}

namespace BookHub.Features.Chat.Service;

using BookHub.Data;
using Data.Models;
using Factories.ChatMessage;
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
    IChatMessageFactory factory,
    ILogger<ChatMessageService> logger) : IChatMessageService
{
    public async Task<ResultWith<ChatMessageServiceModel>> Create(
        CreateChatMessageServiceModel serviceModel,
        CancellationToken token = default)
    {
        var userId = userService.GetId()!;
        var dbModel = serviceModel.ToChatMessageDbModel();
        dbModel.SenderId = userId;

        data.Add(dbModel);
        await data.SaveChangesAsync(token);

        return await this.GetProfileAndBuildResult(
            userId,
            dbModel,
            token);
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
            return this.LogAndReturnUnauthorizedMessage(chatMessageId, userId);
        }

        serviceModel.UpdateChatMessageDbModel(dbModel);
        await data.SaveChangesAsync(token);

        return await this.GetProfileAndBuildResult(
            userId,
            dbModel,
            token);
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

        if (dbModel.SenderId != userId)
        {
            return this.LogAndReturnUnauthorizedMessage(chatMessageId, userId);
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

    private string LogAndReturnUnauthorizedMessage(
        int chatMessageId,
        string userId)
    {
        logger.LogWarning(
            UnauthorizedMessageTemplate,
            userId,
            nameof(ChatMessageDbModel),
            chatMessageId);

        return string.Format(
            UnauthorizedMessage,
            userId,
            nameof(ChatMessageDbModel),
            chatMessageId);
    }

    private async Task<ChatMessageProfileServiceModel?> GetChatMessageProfileServiceModel(
        string userId,
        CancellationToken token = default)
        => await data
            .Profiles
            .Where(p => p.UserId == userId)
            .ToChatMessageProfileServiceModel()
            .FirstOrDefaultAsync(token);

    private ChatMessageServiceModel BuildChatMessageServiceModelResult(
        ChatMessageDbModel chatMessage,
        ChatMessageProfileServiceModel profile)
        => factory
            .WithId(chatMessage.Id)
            .WithMessage(chatMessage.Message)
            .WithSenderId(chatMessage.SenderId)
            .WithSenderName(profile.Name)
            .WithSenderImagePath(profile.ImagePath)
            .CreatedOn(chatMessage.CreatedOn)
            .Build();

    private async Task<ResultWith<ChatMessageServiceModel>> GetProfileAndBuildResult(
        string userId,
        ChatMessageDbModel dbModel,
        CancellationToken token = default)
    {
        var profile = await this.GetChatMessageProfileServiceModel(
            userId,
            token);

        if (profile is null)
        {
            // It should not be possible (in theory) for user without profile to get there.
            // Check to follow good practices and make C# happy without using the null coalescing operator.
            return this.LogAndReturnProfileNotFoundMessage(userId);
        }

        var result = this.BuildChatMessageServiceModelResult(
            dbModel,
            profile);

        return ResultWith<ChatMessageServiceModel>.Success(result);
    }
}

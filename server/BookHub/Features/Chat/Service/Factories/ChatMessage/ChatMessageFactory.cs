namespace BookHub.Features.Chat.Service.Factories.ChatMessage;

using Models;

public class ChatMessageFactory : IChatMessageFactory
{
    private int? id;
    private string? message;
    private string? senderId;
    private string? senderName;
    private string? senderImagePath;
    private DateTime? createdOn;

    public IChatMessageFactory WithId(int id)
    {
        this.id = id;
        return this;
    }

    public IChatMessageFactory WithMessage(string message)
    {
        this.message = message;
        return this;
    }

    public IChatMessageFactory WithSenderId(string senderId)
    {
        this.senderId = senderId;
        return this;
    }

    public IChatMessageFactory WithSenderName(string senderName)
    {
        this.senderName = senderName;
        return this;
    }

    public IChatMessageFactory WithSenderImagePath(string senderImagePath)
    {
        this.senderImagePath = senderImagePath;
        return this;
    }

    public IChatMessageFactory CreatedOn(DateTime createdOn)
    {
        this.createdOn = createdOn;
        return this;
    }

    public ChatMessageServiceModel Build()
    {
        if (this.id == null)
        {
            ArgumentNullException.ThrowIfNull(nameof(this.id));
        }

        if (this.message == null)
        {
            ArgumentNullException.ThrowIfNull(nameof(this.message));
        }

        if (this.senderId == null)
        {
            ArgumentNullException.ThrowIfNull(nameof(this.senderId));
        }

        if (this.senderName == null)
        {
            ArgumentNullException.ThrowIfNull(nameof(this.senderName));
        }

        if (this.senderImagePath == null)
        {
            ArgumentNullException.ThrowIfNull(nameof(this.senderImagePath));
        }

        if (this.createdOn == null)
        {
            ArgumentNullException.ThrowIfNull(nameof(this.createdOn));
        }

        return new()
        {
            Id = this.id!.Value,
            Message = this.message!,
            SenderId = this.senderId!,
            SenderName = this.senderName!,
            SenderImagePath = this.senderImagePath!,
            CreatedOn = this.createdOn!.Value,
        };
    }
}

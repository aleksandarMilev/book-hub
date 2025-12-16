namespace BookHub.Features.Chat.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

public class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessageDbModel>
{
    public void Configure(EntityTypeBuilder<ChatMessageDbModel> builder)
    {
        builder
            .HasIndex(m => new { m.ChatId, m.Id });

        builder
            .HasIndex(m => new { m.ChatId, m.CreatedOn });
    }
}

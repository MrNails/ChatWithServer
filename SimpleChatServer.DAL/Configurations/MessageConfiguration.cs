using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleChatServer.DAL.Models;

namespace SimpleChatServer.DAL.Configurations;

internal class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasKey(b => new { b.ChatId, b.MessageId })
            .HasName("PK_Message_ChatIdMessageId");

        builder.Property(b => b.MessageType)
            .HasColumnType("int");

        builder.Property(b => b.Value)
            .HasMaxLength(500);

        // builder.HasOne(m => m.Chat)
        //     .WithMany(c => c.Messages);
        //
        // builder.HasOne(m => m.User)
        //     .WithMany(u => u.Messages);
    }
}
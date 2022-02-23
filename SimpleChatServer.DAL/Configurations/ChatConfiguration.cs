using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleChatServer.DAL.Models;

namespace SimpleChatServer.DAL.Configurations;

internal class ChatConfiguration : IEntityTypeConfiguration<Chat>
{
    public void Configure(EntityTypeBuilder<Chat> builder)
    {
        builder.HasKey(b => b.Id)
            .HasName("PK_Chat_ChatId");

        builder.Property(b => b.Name)
            .HasColumnType("nvarchar")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property("Created")
            .HasColumnType("datetime");

        builder.Property("Modified")
            .HasColumnType("datetime");
    }
}
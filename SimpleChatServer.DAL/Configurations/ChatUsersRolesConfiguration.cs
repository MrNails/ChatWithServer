using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleChatServer.DAL.Models;

namespace SimpleChatServer.DAL.Configurations;

internal class ChatUsersRolesConfiguration : IEntityTypeConfiguration<ChatUsersRole>
{
    public void Configure(EntityTypeBuilder<ChatUsersRole> builder)
    {
        builder.HasKey(b => new { b.ChatId, b.UserId })
            .HasName("PK_ChatUsersRoles_ChatIdUserId");

        builder.Property(b => b.Restriction)
            .HasColumnType("int");

        builder.Property(b => b.Role)
            .HasColumnType("tinyint");

        // builder.HasOne(cur => cur.Chat)
        //     .WithMany(c => c.ChatUsersRoles);
    }
}
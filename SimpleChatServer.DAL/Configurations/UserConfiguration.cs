using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleChatServer.DAL.Models;

namespace SimpleChatServer.DAL.Configurations;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(uInf => uInf.Id)
            .HasName("PK_UsersInfos_Id");

        builder.Property(uInf => uInf.Login)
            .HasColumnType("nvarchar")
            .HasMaxLength(40);
        
        // builder.Property<string>("Password")
        //     .HasColumnType("nvarchar")
        //     .HasMaxLength(64);

        // builder.HasMany(u => u.Chats)
        //     .WithMany(c => c.Users);
    }
}
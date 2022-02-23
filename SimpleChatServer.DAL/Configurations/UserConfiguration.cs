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

        builder.Property(uInf => uInf.Name)
            .HasColumnType("nvarchar")
            .HasMaxLength(40);

        builder.Property(uInf => uInf.Bio)
            .HasColumnType("nvarchar")
            .HasMaxLength(100);

        builder.Property("Created")
            .HasColumnType("DateTime")
            .ValueGeneratedOnAdd();

        builder.Property("Modified")
            .HasColumnType("DateTime");

        // builder.HasMany(u => u.Chats)
        //     .WithMany(c => c.Users);
    }
}
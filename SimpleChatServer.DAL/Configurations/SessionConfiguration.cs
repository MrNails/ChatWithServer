using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleChatServer.DAL.Models;

namespace SimpleChatServer.DAL.Configurations;

public class SessionConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.HasKey(s => s.UserId)
            .HasName("PK_Sessions_UserId");

        builder.Property(s => s.Token)
            .HasColumnType("nvarchar")
            .HasMaxLength(300);
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleChatServer.DAL.Models;

namespace SimpleChatServer.DAL.Configurations;

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.HasKey(up => up.UserId)
            .HasName("PK_UserProfile_UserId");
        
        builder.Property(uInf => uInf.Bio)
            .HasColumnType("nvarchar")
            .HasMaxLength(100);
    }
}
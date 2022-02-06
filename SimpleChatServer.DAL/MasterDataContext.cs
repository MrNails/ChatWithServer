using Microsoft.EntityFrameworkCore;
using SimpleChatServer.DAL.Models;

namespace SimpleChatServer.DAL
{
    public class MasterDataContext : DbContext
    {
        private string m_connectionString;
        
        public DbSet<User> Users { get; set; }
        public DbSet<Chat> Chats { get; set; }

        public MasterDataContext(string connectionString)
        {
            m_connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(m_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var uInfEntityBuilder = modelBuilder.Entity<User>();
            
            uInfEntityBuilder.HasKey(uInf => uInf.Id)
                .HasName("PK_UsersInfos_Id");

            uInfEntityBuilder.Property(uInf => uInf.Name)
                .HasColumnType("nvarchar")
                .HasMaxLength(40);

            uInfEntityBuilder.Property(uInf => uInf.Bio)
                .HasColumnType("nvarchar")
                .HasMaxLength(100);

            uInfEntityBuilder.Property("Created")
                .HasColumnType("DateTime")
                .ValueGeneratedOnAdd();
        }

        public override int SaveChanges()
        {
            // var selectedEntityList = ChangeTracker.Entries()
            //     .Where(entry => entry is  entry.State is EntityState.Added or EntityState.Modified);
            //
            //
            // selectedEntityList.ForEach(entity =>
            // {
            //     if (entity.State == EntityState.Added)
            //     {
            //         if (entity.Entity is IEntityCreatedAt)
            //             ((IEntityCreatedAt)entity.Entity).CreatedAt = DateTimeOffset.UtcNow;
            //     }
            //
            //     if (entity.State == EntityState.Modified)
            //     {
            //         if (entity.Entity is IEntityModifiedAt)
            //             ((IEntityModifiedAt)entity.Entity).ModifiedAt = DateTimeOffset.UtcNow;
            //
            //         if (entity.Entity is IEntityIsDeleted)
            //             if (((IEntityIsDeleted)entity.Entity).IsDeleted)
            //                 ((IEntityIsDeleted)entity.Entity).DeletedAt = DateTimeOffset.UtcNow;
            //     }
            // });
            
            return base.SaveChanges();
        }
    }
}
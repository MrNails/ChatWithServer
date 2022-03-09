using Microsoft.EntityFrameworkCore;
using SimpleChatServer.DAL.Models;

namespace SimpleChatServer.DAL
{
    public class MasterDataContext : DbContext
    {
        private readonly string m_connectionString;
        
        public DbSet<User?> Users { get; set; }
        public DbSet<Chat?> Chats { get; set; }
        public DbSet<Message?> Messages { get; set; }
        public DbSet<ChatUserRole?> ChatUsersRoles { get; set; }
        public DbSet<Session> Sessions { get; set; }

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
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MasterDataContext).Assembly);
        }
    }
}
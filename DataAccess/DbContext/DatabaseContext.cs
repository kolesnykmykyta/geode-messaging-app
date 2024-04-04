using DataAccess.Configurations;
using DataAccess.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DbContext
{
    public class DatabaseContext : IdentityDbContext<User>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        DbSet<Chat> Chats { get; set; }

        DbSet<ChatMember> ChatMembers { get; set; }

        DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new ChatEntityConfiguration());
            builder.ApplyConfiguration(new ChatMemberEntityConfiguration());
            builder.ApplyConfiguration(new MessageEntityConfiguration());
        }
    }
}

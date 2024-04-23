﻿using DataAccess.Configurations;
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

        public DatabaseContext() { }

        DbSet<Chat> Chats { get; set; }

        DbSet<ChatMember> ChatMembers { get; set; }

        DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new ChatEntityConfiguration());
            builder.ApplyConfiguration(new ChatMemberEntityConfiguration());
            builder.ApplyConfiguration(new MessageEntityConfiguration());

            builder.Entity<User>(e =>
            {
                e.HasIndex(u => u.Email).IsUnique();
            });
        }
    }
}

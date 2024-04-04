using DataAccess.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    internal class ChatEntityConfiguration : BaseEntityConfiguration<Chat>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Chat> builder)
        {
            builder.Property(c => c.Name).IsRequired();
            builder.Property(c => c.Name).HasMaxLength(30);
            builder.Property(c => c.ChatOwnerId).IsRequired();
        }
    }
}

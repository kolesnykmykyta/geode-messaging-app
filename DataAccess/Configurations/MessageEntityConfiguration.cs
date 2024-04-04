using DataAccess.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    internal class MessageEntityConfiguration : BaseEntityConfiguration<Message>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Message> builder)
        {
            builder.Property(m => m.ChatId).IsRequired();
            builder.Property(m => m.SenderId).IsRequired();
            builder.Property(m => m.Content).IsRequired();
            builder.Property(m => m.Content).HasMaxLength(300);
            builder.Property(m => m.SentAt).IsRequired();
        }
    }
}

using DataAccess.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    internal class ChatMemberEntityConfiguration : BaseEntityConfiguration<ChatMember>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<ChatMember> builder)
        {
            builder.Property(m => m.ChatId).IsRequired();
            builder.Property(m => m.UserId).IsRequired();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class ChatMember : BaseEntity
    {
        [ForeignKey("User")]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("Chat")]
        public int ChatId { get; set; }

        // Navigation properties
        public User User { get; set; }

        public Chat Chat { get; set; }
    }
}

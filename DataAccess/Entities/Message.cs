using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class Message
    {
        public int Id { get; set; }

        [ForeignKey("Chat")]
        public int ChatId { get; set; }

        [ForeignKey("Sender")]
        public string SenderId { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public DateTime SentAt { get; set; }

        // Navigation properties
        public User Sender { get; set; }

        public Chat Chat { get; set; }
    }
}

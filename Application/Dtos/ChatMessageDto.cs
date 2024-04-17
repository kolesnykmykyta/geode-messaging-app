using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class ChatMessageDto
    {
        public string? Sender { get; set; }

        public string? Content { get; set; }

        public DateTime? SentAt { get; set; }
    }
}

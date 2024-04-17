﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class ChatMessageDto
    {
        [JsonPropertyName("sender")]
        public string? Sender { get; set; }

        [JsonPropertyName("content")]
        public string? Content { get; set; }

        [JsonPropertyName("sentAt")]
        public DateTime? SentAt { get; set; }
    }
}

using Application.Dtos;
using MediatR;

namespace Application.Services.Messages
{
    public class SendMessageCommand : IRequest
    {
        public int ChatId { get; set; }

        public string SenderId { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;
    }
}

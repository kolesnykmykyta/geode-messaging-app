using Application.Dtos;
using DataAccess.Entities;
using MediatR;

namespace Application.Services.Messages
{
    public class GetChatMessagesQuery : IRequest<IEnumerable<ChatMessageDto>?>
    {
        public GetChatMessagesQuery(int chatId)
        {
            ChatId = chatId;
        }

        public int ChatId { get; set; }
    }
}

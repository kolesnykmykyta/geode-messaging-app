using DataAccess.Entities;
using MediatR;

namespace Application.Services.Chats
{
    public class GetChatMessagesQuery : IRequest<IEnumerable<Message>>
    {
    }
}

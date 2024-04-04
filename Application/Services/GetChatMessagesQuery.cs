using DataAccess.Entities;
using MediatR;

namespace Application.Services
{
    public class GetChatMessagesQuery : IRequest<IEnumerable<Message>>
    {
    }
}

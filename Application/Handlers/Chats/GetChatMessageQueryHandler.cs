using Application.Services.Chats;
using DataAccess.Entities;
using MediatR;

namespace Application.Handlers.Chats
{
    public class GetChatMessageQueryHandler : IRequestHandler<GetChatMessagesQuery, IEnumerable<Message>>
    {
        public Task<IEnumerable<Message>> Handle(GetChatMessagesQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

using Application.Services;
using DataAccess.Entities;
using MediatR;

namespace Application.Handlers
{
    public class GetChatMessageQueryHandler : IRequestHandler<GetChatMessagesQuery, IEnumerable<Message>>
    {
        public Task<IEnumerable<Message>> Handle(GetChatMessagesQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

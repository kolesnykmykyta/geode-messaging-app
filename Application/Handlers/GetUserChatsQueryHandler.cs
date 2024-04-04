using Application.Services;
using DataAccess.Entities;
using MediatR;

namespace Application.Handlers
{
    public class GetUserChatsQueryHandler : IRequestHandler<GetUserChatsQuery, IEnumerable<Chat>>
    {
        public Task<IEnumerable<Chat>> Handle(GetUserChatsQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

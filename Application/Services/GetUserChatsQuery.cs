using DataAccess.Entities;
using MediatR;

namespace Application.Services
{
    public class GetUserChatsQuery : IRequest<IEnumerable<Chat>>
    {
    }
}

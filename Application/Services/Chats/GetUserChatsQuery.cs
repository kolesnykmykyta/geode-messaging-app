using Application.Dtos;
using DataAccess.Entities;
using MediatR;

namespace Application.Services.Chats
{
    public class GetUserChatsQuery : IRequest<IEnumerable<ChatDto>>
    {
        public string UserId { get; set; } = string.Empty;

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string? SearchParam { get; set; }

        public string? SortProp { get; set; }

        public bool SortByDescending { get; set; }

        public string? SelectProps { get; set; }
    }
}

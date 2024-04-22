using MediatR;

namespace Application.Services.Chats
{
    public class LeaveChatCommand : IRequest<bool>
    {
        public int ChatId { get; set; }

        public string UserId { get; set; } = string.Empty;
    }
}

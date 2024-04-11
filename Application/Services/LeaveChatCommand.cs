using MediatR;

namespace Application.Services
{
    public class LeaveChatCommand : IRequest<bool>
    {
        public int ChatId { get; set; }

        public string UserId { get; set; } = string.Empty;
    }
}

using MediatR;

namespace Application.Services.Chats
{
    public class CreateChatCommand : IRequest
    {
        public string Name { get; set; } = string.Empty;

        public string ChatOwnerId { get; set; } = string.Empty;
    }
}
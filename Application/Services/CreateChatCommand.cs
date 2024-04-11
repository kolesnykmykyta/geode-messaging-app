using MediatR;

namespace Application.Services
{
    public class CreateChatCommand : IRequest<bool>
    {
        public string Name { get; set; } = string.Empty;

        public string ChatOwnerId { get; set; } = string.Empty;
    }
}
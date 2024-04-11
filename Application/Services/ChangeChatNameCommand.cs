using MediatR;

namespace Application.Services
{
    public class ChangeChatNameCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string ChatOwnerId { get; set; } = string.Empty;
    }
}
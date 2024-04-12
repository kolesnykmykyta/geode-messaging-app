using MediatR;

namespace Application.Services.Messages
{
    public class SendMessageCommand : IRequest<bool>
    {
    }
}

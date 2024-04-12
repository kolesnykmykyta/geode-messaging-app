using Application.Services.Messages;
using MediatR;

namespace Application.Handlers.Messages
{
    public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, bool>
    {
        public Task<bool> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

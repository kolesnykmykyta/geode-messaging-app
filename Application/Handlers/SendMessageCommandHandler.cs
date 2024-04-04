using Application.Services;
using MediatR;

namespace Application.Handlers
{
    public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, bool>
    {
        public Task<bool> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

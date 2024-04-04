using Application.Services;
using MediatR;

namespace Application.Handlers
{
    public class JoinChatCommandHandler : IRequestHandler<JoinChatCommand, bool>
    {
        public Task<bool> Handle(JoinChatCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

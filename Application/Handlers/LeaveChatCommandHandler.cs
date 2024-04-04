using Application.Services;
using MediatR;

namespace Application.Handlers
{
    public class LeaveChatCommandHandler : IRequestHandler<LeaveChatCommand>
    {
        public Task Handle(LeaveChatCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

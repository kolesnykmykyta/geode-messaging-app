using Application.Services;
using MediatR;

namespace Application.Handlers
{
    public class ChangeChatNameCommandHandler : IRequestHandler<ChangeChatNameCommand, bool>
    {
        public Task<bool> Handle(ChangeChatNameCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

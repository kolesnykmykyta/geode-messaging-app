using Application.Services;
using MediatR;

namespace Application.Handlers
{
    public class CreateChatCommandHandler : IRequestHandler<CreateChatCommand, bool>
    {
        public Task<bool> Handle(CreateChatCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

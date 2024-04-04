using Application.Services;
using MediatR;

namespace Application.Handlers
{
    public class UpdateUsernameCommandHandler : IRequestHandler<UpdateUsernameCommand, bool>
    {
        public Task<bool> Handle(UpdateUsernameCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

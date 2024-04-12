using Application.Services.Users;
using MediatR;

namespace Application.Handlers.Users
{
    public class UpdateUsernameCommandHandler : IRequestHandler<UpdateUsernameCommand, bool>
    {
        public Task<bool> Handle(UpdateUsernameCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

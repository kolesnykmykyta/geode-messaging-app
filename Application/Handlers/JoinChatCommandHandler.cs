using Application.Services;
using Application.Utils.Helpers.Interfaces;
using DataAccess.UnitOfWork;
using MediatR;

namespace Application.Handlers
{
    public class JoinChatCommandHandler : IRequestHandler<JoinChatCommand, bool>
    {
        private readonly IChatRepositoryHelper _helper;

        public JoinChatCommandHandler(IMediator mediator, IChatRepositoryHelper helper)
        {
            _helper = helper;
        }

        public async Task<bool> Handle(JoinChatCommand request, CancellationToken cancellationToken)
        {
            return _helper.JoinChat(request.ChatId, request.UserId);
        }
    }
}

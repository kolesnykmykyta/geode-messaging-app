using Application.Services.Chats;
using Application.Utils.Helpers.Interfaces;
using DataAccess.UnitOfWork;
using MediatR;

namespace Application.Handlers.Chats
{
    public class JoinChatCommandHandler : IRequestHandler<JoinChatCommand, bool>
    {
        private readonly IChatRepositoryHelper _helper;

        public JoinChatCommandHandler(IChatRepositoryHelper helper)
        {
            _helper = helper;
        }

        public async Task<bool> Handle(JoinChatCommand request, CancellationToken cancellationToken)
        {
            return _helper.JoinChat(request.ChatId, request.UserId);
        }
    }
}

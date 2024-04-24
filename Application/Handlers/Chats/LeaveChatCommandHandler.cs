using Application.Services.Chats;
using Application.Utils.Helpers.Interfaces;
using MediatR;

namespace Application.Handlers.Chats
{
    public class LeaveChatCommandHandler : IRequestHandler<LeaveChatCommand, bool>
    {
        private readonly IChatRepositoryHelper _helper;

        public LeaveChatCommandHandler(IChatRepositoryHelper helper)
        {
            _helper = helper;
        }
        public async Task<bool> Handle(LeaveChatCommand request, CancellationToken cancellationToken)
        {
            return _helper.LeaveChat(request.ChatId, request.UserId);
        }
    }
}

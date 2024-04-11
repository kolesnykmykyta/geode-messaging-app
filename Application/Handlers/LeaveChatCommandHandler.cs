using Application.Services;
using Application.Utils.Helpers.Interfaces;
using MediatR;

namespace Application.Handlers
{
    public class LeaveChatCommandHandler : IRequestHandler<LeaveChatCommand, bool>
    {
        private readonly IChatRepositoryHelper _helper;

        public LeaveChatCommandHandler(IMediator mediator, IChatRepositoryHelper helper)
        {
            _helper = helper;
        }
        public async Task<bool> Handle(LeaveChatCommand request, CancellationToken cancellationToken)
        {
            return _helper.LeaveChat(request.ChatId, request.UserId);
        }
    }
}

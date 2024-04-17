using Application.Dtos;
using Application.Services.Messages;
using Application.Utils.Helpers.Interfaces;
using DataAccess.Entities;
using DataAccess.UnitOfWork;
using MediatR;

namespace Application.Handlers.Messages
{
    public class GetChatMessagesQueryHandler : IRequestHandler<GetChatMessagesQuery, IEnumerable<ChatMessageDto>>
    {
        private readonly IChatRepositoryHelper _helper;

        public GetChatMessagesQueryHandler(IChatRepositoryHelper helper)
        {
            _helper = helper;
        }

        public async Task<IEnumerable<ChatMessageDto>> Handle(GetChatMessagesQuery request, CancellationToken cancellationToken)
        {
            return _helper.GetMessagesInChat(request.ChatId);
        }
    }
}

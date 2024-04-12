using Application.Dtos;
using Application.Services.Chats;
using Application.Utils.Helpers.Interfaces;
using AutoMapper;
using DataAccess.Entities;
using DataAccess.UnitOfWork;
using MediatR;

namespace Application.Handlers.Chats
{
    public class GetUserChatsQueryHandler : IRequestHandler<GetUserChatsQuery, IEnumerable<ChatDto>>
    {
        private readonly IChatRepositoryHelper _chatHelper;
        private readonly IMapper _mapper;
        private readonly IRepositoryParametersHelper _parametersHelper;

        public GetUserChatsQueryHandler(IChatRepositoryHelper chatHelper, IMapper mapper, IRepositoryParametersHelper parametersHelper)
        {
            _chatHelper = chatHelper;
            _mapper = mapper;
            _parametersHelper = parametersHelper;
        }

        public async Task<IEnumerable<ChatDto>> Handle(GetUserChatsQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<string>? selectPropsList = _parametersHelper.SplitSelectProperties(request.SelectProps);
            Dictionary<string, string> searchParameters = _parametersHelper.GenerateSearchParametersDictionary(request.SearchParam);

            IEnumerable<Chat> chatList = _chatHelper
                .GetUserChats(request.UserId, searchParameters, request.SortProp, request.SortByDescending, request.PageSize, request.PageNumber, selectPropsList);

            return _mapper.Map<IEnumerable<ChatDto>>(chatList);
        }
    }
}

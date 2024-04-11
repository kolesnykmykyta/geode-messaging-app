using Application.Dtos;
using Application.Services;
using AutoMapper;
using DataAccess.Entities;
using DataAccess.UnitOfWork;
using MediatR;

namespace Application.Handlers
{
    public class GetUserChatsQueryHandler : IRequestHandler<GetUserChatsQuery, IEnumerable<ChatDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUserChatsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ChatDto>> Handle(GetUserChatsQuery request, CancellationToken cancellationToken)
        {
            List<string>? selectPropsList = request.SelectProps?.Split(",").ToList();
            Dictionary<string, string>? searchParameters = new()
            {
                {"OwnerId", request.OwnerId }
            };

            if (request.SearchParam != null)
            {
                searchParameters = new Dictionary<string, string>()
                    {
                        {"all", request.SearchParam }
                    };
            }

            IEnumerable<ChatMember> chatMembers = _unitOfWork.GenericRepository<ChatMember>().GetList();

            IEnumerable<Chat> chatList = _unitOfWork.GenericRepository<Chat>()
                .GetList(searchParameters, request.SortProp, request.SortByDescending, request.PageSize, request.PageNumber, selectPropsList);

            return _mapper.Map<IEnumerable<ChatDto>>(chatList);
        }
    }
}

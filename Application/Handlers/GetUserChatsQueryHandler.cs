using Application.Dtos;
using Application.Services;
using Application.Utils.Helpers.Interfaces;
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
        private readonly IRepositoryParametersHelper _parametersHelper;

        public GetUserChatsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IRepositoryParametersHelper parametersHelper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _parametersHelper = parametersHelper;
        }

        public async Task<IEnumerable<ChatDto>> Handle(GetUserChatsQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<string>? selectPropsList = _parametersHelper.SplitSelectProperties(request.SelectProps);
            Dictionary<string, string> searchParameters = _parametersHelper.GenerateSearchParametersDictionary(request.SearchParam);
            searchParameters["OwnerId"] = request.OwnerId;

            IEnumerable<Chat> chatList = _unitOfWork.GenericRepository<Chat>()
                .GetList(searchParameters, request.SortProp, request.SortByDescending, request.PageSize, request.PageNumber, selectPropsList);

            return _mapper.Map<IEnumerable<ChatDto>>(chatList);
        }
    }
}

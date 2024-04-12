using Application.Dtos;
using Application.Services.Messages;
using Application.Utils.Helpers.Interfaces;
using AutoMapper;
using DataAccess.Entities;
using DataAccess.UnitOfWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.Messages
{
    public class GetUserMessagesQueryHandler : IRequestHandler<GetUserMessagesQuery, IEnumerable<MessageDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepositoryParametersHelper _parametersHelper;

        public GetUserMessagesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IRepositoryParametersHelper parametersHelper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _parametersHelper = parametersHelper;
        }

        public async Task<IEnumerable<MessageDto>> Handle(GetUserMessagesQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<string>? selectPropsList = _parametersHelper.SplitSelectProperties(request.SelectProps);
            Dictionary<string, string>? searchParameters = _parametersHelper.GenerateSearchParametersDictionary(request.SearchParam);
            searchParameters["SenderId"] = request.SenderId;

            IEnumerable<Message> messagesList = _unitOfWork.GenericRepository<Message>()
                .GetList(searchParameters, request.SortProp, request.SortByDescending, request.PageSize, request.PageNumber, selectPropsList);

            return _mapper.Map<IEnumerable<MessageDto>>(messagesList);
        }
    }
}

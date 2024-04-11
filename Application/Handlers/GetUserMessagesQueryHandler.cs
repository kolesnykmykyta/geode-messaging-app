using Application.Dtos;
using Application.Services;
using AutoMapper;
using DataAccess.Entities;
using DataAccess.UnitOfWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers
{
    public class GetUserMessagesQueryHandler : IRequestHandler<GetUserMessagesQuery, IEnumerable<MessageDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUserMessagesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MessageDto>> Handle(GetUserMessagesQuery request, CancellationToken cancellationToken)
        {
            List<string>? selectPropsList = request.SelectProps?.Split(",").ToList();
            Dictionary<string, string>? searchParameters = new()
            {
                {"SenderId", request.SenderId }
            };

            if (request.SearchParam != null)
            {
                searchParameters = new Dictionary<string, string>()
                    {
                        {"all", request.SearchParam }
                    };
            }

            IEnumerable<Message> messagesList = _unitOfWork.GenericRepository<Message>()
                .GetList(searchParameters, request.SortProp, request.SortByDescending, request.PageSize, request.PageNumber, selectPropsList);

            return _mapper.Map<IEnumerable<MessageDto>>(messagesList);
        }
    }
}

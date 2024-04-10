using Application.Dtos;
using Application.Services;
using AutoMapper;
using DataAccess.DbContext;
using DataAccess.Entities;
using DataAccess.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers
{
    public class GetUsersListQueryHandler : IRequestHandler<GetUsersListQuery, IEnumerable<UserInfoDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUsersListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {   
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserInfoDto>> Handle(GetUsersListQuery request, CancellationToken cancellationToken)
        {
            List<string>? selectPropsList = request.SelectProps?.Split(",").ToList();
            Dictionary<string, string>? searchParameters = null;
            if (request.SearchParam != null)
            {
                searchParameters = new Dictionary<string, string>()
                    {
                        {"all", request.SearchParam }
                    };
            }

            IEnumerable<User> returnList = _unitOfWork.GenericRepository<User>()
                .GetList(searchParameters, request.SortProp, request.SortByDescending, request.PageSize, request.PageNumber, selectPropsList);
            return _mapper.Map<IEnumerable<UserInfoDto>>(returnList);
        }
    }
}

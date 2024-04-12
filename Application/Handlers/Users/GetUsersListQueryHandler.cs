using Application.Dtos;
using Application.Services.Users;
using Application.Utils.Helpers.Interfaces;
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

namespace Application.Handlers.Users
{
    public class GetUsersListQueryHandler : IRequestHandler<GetUsersListQuery, IEnumerable<UserInfoDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepositoryParametersHelper _parametersHelper;

        public GetUsersListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IRepositoryParametersHelper parametersHelper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _parametersHelper = parametersHelper;
        }

        public async Task<IEnumerable<UserInfoDto>> Handle(GetUsersListQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<string>? selectPropsList = _parametersHelper.SplitSelectProperties(request.SelectProps);
            Dictionary<string, string>? searchParameters = _parametersHelper.GenerateSearchParametersDictionary(request.SearchParam);

            IEnumerable<User> returnList = _unitOfWork.GenericRepository<User>()
                .GetList(searchParameters, request.SortProp, request.SortByDescending, request.PageSize, request.PageNumber, selectPropsList);
            return _mapper.Map<IEnumerable<UserInfoDto>>(returnList);
        }
    }
}

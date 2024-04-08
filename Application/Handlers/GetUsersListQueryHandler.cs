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
            IEnumerable<User> returnList = _unitOfWork.GenericRepository<User>().GetList(request.SearchParam, request.PageSize, request.PageNumber);
            return _mapper.Map<IEnumerable<UserInfoDto>>(returnList);
        }
    }
}

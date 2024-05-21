using Application.CqrsMessages.Users;
using Application.Dtos;
using AutoMapper;
using DataAccess.Entities;
using DataAccess.UnitOfWork;
using MediatR;
using System.Runtime.CompilerServices;

namespace Application.Handlers.Users
{
    public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, UserProfileDto>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetUserProfileQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<UserProfileDto> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            User? userInfo = _unitOfWork.GenericRepository<User>().GetById(request.UserId);
            return _mapper.Map<UserProfileDto>(userInfo);
        }
    }
}

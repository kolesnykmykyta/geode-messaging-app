using Application.CqrsMessages.Users;
using AutoMapper;
using DataAccess.Entities;
using DataAccess.UnitOfWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.Users
{
    public class UpdateUserDataCommandHandler : IRequestHandler<UpdateUserDataCommand, bool>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUserDataCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateUserDataCommand request, CancellationToken cancellationToken)
        {
            User updatedUser = _mapper.Map<User>(request);
            updatedUser.NormalizedUserName = request.UserName.ToUpper();

            _unitOfWork.GenericRepository<User>().Update(request.Id, updatedUser);

            try
            {
                _unitOfWork.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

using Application.Services;
using AutoMapper;
using DataAccess.Entities;
using DataAccess.UnitOfWork;
using MediatR;

namespace Application.Handlers
{
    public class CreateChatCommandHandler : IRequestHandler<CreateChatCommand, bool>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unifOfWork;
        
        public CreateChatCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unifOfWork = unitOfWork;
        }

        public async Task<bool> Handle(CreateChatCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Chat chatToCreate = _mapper.Map<Chat>(request);
                _unifOfWork.GenericRepository<Chat>().Insert(chatToCreate);
                _unifOfWork.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

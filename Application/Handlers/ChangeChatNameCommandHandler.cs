using Application.Services;
using AutoMapper;
using DataAccess.Entities;
using DataAccess.UnitOfWork;
using MediatR;

namespace Application.Handlers
{
    public class ChangeChatNameCommandHandler : IRequestHandler<ChangeChatNameCommand, bool>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unifOfWork;

        public ChangeChatNameCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unifOfWork = unitOfWork;
        }

        public async Task<bool> Handle(ChangeChatNameCommand request, CancellationToken cancellationToken)
        {
            Chat? existingChat = _unifOfWork.GenericRepository<Chat>().GetById(request.Id);

            if (existingChat == null || existingChat.ChatOwnerId != request.ChatOwnerId)
            {
                return false;
            }
            else
            {
                _unifOfWork.GenericRepository<Chat>().Update(request.Id, _mapper.Map<Chat>(request));
                _unifOfWork.SaveChanges();
                return true;
            }
        }
    }
}

using Application.Services.Chats;
using AutoMapper;
using DataAccess.Entities;
using DataAccess.UnitOfWork;
using MediatR;

namespace Application.Handlers.Chats
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
            Chat chatToCreate = _mapper.Map<Chat>(request);
            ChatMember member = new ChatMember() { UserId = request.ChatOwnerId, Chat = chatToCreate};
            _unifOfWork.GenericRepository<Chat>().Insert(chatToCreate);
            _unifOfWork.GenericRepository<ChatMember>().Insert(member);
            _unifOfWork.SaveChanges();
            return true;
        }
    }
}

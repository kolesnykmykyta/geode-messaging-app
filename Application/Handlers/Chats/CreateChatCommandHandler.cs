using Application.Services.Chats;
using Application.Utils.Helpers.Interfaces;
using AutoMapper;
using DataAccess.Entities;
using DataAccess.UnitOfWork;
using MediatR;

namespace Application.Handlers.Chats
{
    public class CreateChatCommandHandler : IRequestHandler<CreateChatCommand, bool>
    {
        private readonly IMapper _mapper;
        private readonly IChatRepositoryHelper _repositoryHelper;

        public CreateChatCommandHandler(IMapper mapper, IChatRepositoryHelper repositoryHelper)
        {
            _mapper = mapper;
            _repositoryHelper = repositoryHelper;
        }

        public async Task<bool> Handle(CreateChatCommand request, CancellationToken cancellationToken)
        {
            Chat chatToCreate = _mapper.Map<Chat>(request);
            return _repositoryHelper.CreateNewChat(chatToCreate);
        }
    }
}

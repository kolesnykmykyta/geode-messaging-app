using Application.Services.Chats;
using DataAccess.Entities;
using DataAccess.UnitOfWork;
using MediatR;

namespace Application.Handlers.Chats
{
    public class DeleteChatCommandHandler : IRequestHandler<DeleteChatCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteChatCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteChatCommand request, CancellationToken cancellationToken)
        {
            Chat? existingChat = _unitOfWork.GenericRepository<Chat>().GetById(request.ChatId);
            if (existingChat != null && existingChat.ChatOwnerId == request.UserId)
            {
                _unitOfWork.GenericRepository<Chat>().Delete(request.ChatId);
                _unitOfWork.SaveChanges();
                return true;
            }

            return false;
        }
    }
}

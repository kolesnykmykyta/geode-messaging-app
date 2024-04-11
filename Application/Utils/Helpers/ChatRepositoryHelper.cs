using Application.Utils.Helpers.Interfaces;
using DataAccess.Entities;
using DataAccess.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utils.Helpers
{
    public class ChatRepositoryHelper : IChatRepositoryHelper
    {
        private readonly IUnitOfWork _unitOfWork;

        public ChatRepositoryHelper(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool JoinChat(int chatId, string userId)
        {
            Chat? existingChat = _unitOfWork.GenericRepository<Chat>().GetById(chatId);
            if (existingChat == null)
            {
                return false;
            }
            else
            {
                _unitOfWork.GenericRepository<ChatMember>().Insert(new ChatMember { ChatId = chatId, UserId = userId });
                _unitOfWork.SaveChanges();
                return true;
            }
        }

        public bool LeaveChat(int chatId, string userId)
        {
            ChatMember? existingChatMembership = _unitOfWork.GenericRepository<ChatMember>()
                .GetList()
                .Where(m => m.ChatId == chatId && m.UserId == userId)
                .FirstOrDefault();

            if (existingChatMembership == null)
            {
                return false;
            }
            else
            {
                Chat? existingChat = _unitOfWork.GenericRepository<Chat>().GetById(chatId);
                if (existingChat == null || existingChat.ChatOwnerId == userId)
                {
                    return false;
                }

                else
                {
                    _unitOfWork.GenericRepository<ChatMember>().Delete(existingChatMembership.Id);
                    _unitOfWork.SaveChanges();
                    return true;
                }
            }
        }
    }
}

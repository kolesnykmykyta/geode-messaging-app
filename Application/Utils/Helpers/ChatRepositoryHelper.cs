using Application.Dtos;
using Application.Utils.Helpers.Interfaces;
using AutoMapper;
using DataAccess.Entities;
using DataAccess.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
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
        private readonly IMapper _mapper;

        public ChatRepositoryHelper(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public void CreateNewChat(Chat newChat)
        {
            ChatMember member = new ChatMember() { UserId = newChat.ChatOwnerId, Chat = newChat };
            _unitOfWork.GenericRepository<Chat>().Insert(newChat);
            _unitOfWork.GenericRepository<ChatMember>().Insert(member);
            _unitOfWork.SaveChanges();
        }

        public IEnumerable<ChatDto> GetUserChats
            (string userId,
            Dictionary<string, string>? searchParams = null,
            string? sortingProp = null,
            bool sortDescending = false,
            int? pageSize = null,
            int? pageNumber = null,
            IEnumerable<string>? selectProps = null)
        {
            IQueryable<Chat> chatList = _unitOfWork.GenericRepository<Chat>()
                .GetList(searchParams, sortingProp, sortDescending, null, null, selectProps)
                .Include(c => c.ChatMembers)
                .Where(c => c.ChatMembers.Any(cm => cm.UserId == userId));

            if (pageSize != null && pageNumber != null)
            {
                chatList = chatList.Skip(((int)pageNumber - 1) * (int)pageSize)
                                .Take((int)pageSize);
            }

            List<ChatDto> chatDtos = new List<ChatDto>();
            foreach(Chat chat in chatList)
            {
                ChatDto dto = _mapper.Map<ChatDto>(chat);
                dto.IsUserOwner = userId == chat.ChatOwnerId;
                chatDtos.Add(dto);
            }

            return chatDtos;
        }

        public bool JoinChat(int chatId, string userId)
        {
            Chat? existingChat = _unitOfWork.GenericRepository<Chat>().GetById(chatId);
            ChatMember? existingChatMember = _unitOfWork.GenericRepository<ChatMember>()
                .GetList()
                .Where(cm => cm.ChatId == chatId && cm.UserId == userId)
                .FirstOrDefault();

            if (existingChat == null || existingChatMember != null)
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

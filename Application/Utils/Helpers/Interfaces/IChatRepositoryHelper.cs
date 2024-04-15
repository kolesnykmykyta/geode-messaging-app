using Application.Dtos;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utils.Helpers.Interfaces
{
    public interface IChatRepositoryHelper
    {
        IEnumerable<ChatDto> GetUserChats
            (string userId,
            Dictionary<string, string>? searchParams = null,
            string? sortingProp = null,
            bool sortDescending = false,
            int? pageSize = null,
            int? pageNumber = null,
            IEnumerable<string>? selectProps = null);

        void CreateNewChat(Chat newChat);

        bool LeaveChat(int chatId, string userId);

        bool JoinChat(int chatId, string userId);
    }
}

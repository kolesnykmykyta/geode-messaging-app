﻿using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utils.Helpers.Interfaces
{
    public interface IChatRepositoryHelper
    {
        IEnumerable<Chat> GetUserChats
            (string userId,
            Dictionary<string, string>? searchParams = null,
            string? sortingProp = null,
            bool sortDescending = false,
            int? pageSize = null,
            int? pageNumber = null,
            IEnumerable<string>? selectProps = null);

        bool LeaveChat(int chatId, string userId);

        bool JoinChat(int chatId, string userId);
    }
}

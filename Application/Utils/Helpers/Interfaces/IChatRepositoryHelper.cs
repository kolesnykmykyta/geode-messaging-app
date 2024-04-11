﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utils.Helpers.Interfaces
{
    public interface IChatRepositoryHelper
    {
        bool LeaveChat(int chatId, string userId);

        bool JoinChat(int chatId, string userId);
    }
}

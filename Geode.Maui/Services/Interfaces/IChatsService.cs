using Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geode.Maui.Services.Interfaces
{
    internal interface IChatsService
    {
        Task<IEnumerable<ChatDto>> GetAllUserChatsAsync(FilterDto? filter);

        Task<bool> CreateNewChat(string chatName);

        Task<bool> JoinChatAsync(string? chatId);

        Task<bool> LeaveChatAsync(string? chatId);
    }
}

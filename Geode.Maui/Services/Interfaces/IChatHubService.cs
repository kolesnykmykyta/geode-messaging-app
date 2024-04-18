using Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geode.Maui.Services.Interfaces
{
    internal interface IChatHubService
    {
        event Action<ChatMessageDto> OnMessageReceived;

        Task ConnectToChatHubAsync(int chatId);

        Task SendMessageAsync(int chatId, string message);
    }
}

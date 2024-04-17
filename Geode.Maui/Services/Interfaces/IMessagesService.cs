using Application.Dtos;

namespace Geode.Maui.Services.Interfaces
{
    internal interface IMessagesService
    {
        Task<IEnumerable<MessageDto>> GetAllUserMessagesAsync(FilterDto? filter);

        Task<IEnumerable<ChatMessageDto>> GetAllChatMessages(int chatId);
    }
}

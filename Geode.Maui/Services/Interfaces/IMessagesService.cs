using Application.Dtos;

namespace Geode.Maui.Services.Interfaces
{
    internal interface IMessagesService
    {
        Task<IEnumerable<MessageDto>> GetAllUserMessages(FilterDto? filter);
    }
}

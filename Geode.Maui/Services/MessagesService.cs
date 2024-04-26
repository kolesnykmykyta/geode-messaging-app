using Application.Dtos;
using Application.Utils.HttpClientWrapper;
using Blazored.LocalStorage;
using Geode.Maui.Services.Interfaces;
using System.Reflection;
using System.Text.Json;

namespace Geode.Maui.Services
{
    internal class MessagesService : IMessagesService
    {
        private readonly IHttpClientWrapper _httpClient;
        private readonly IServicesHelper _helper;

        public MessagesService(IHttpClientWrapper httpClient, IServicesHelper helper)
        {
            _httpClient = httpClient;
            _helper = helper;
        }

        public async Task<IEnumerable<ChatMessageDto>> GetAllChatMessages(int chatId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"messages/chat/{chatId}", null, await _helper.GetAccessTokenAsync());

            if (response.IsSuccessStatusCode)
            {
                return await _helper.DeserializeJsonAsync<IEnumerable<ChatMessageDto>>(response);
            }
            else
            {
                return new List<ChatMessageDto>();
            }
        }

        public async Task<IEnumerable<MessageDto>> GetAllUserMessagesAsync(FilterDto? filter)
        {
            Dictionary<string, string>? queryParams = filter == null ? null : _helper.CreateDictionaryFromObject(filter);
            HttpResponseMessage response = await _httpClient.GetAsync("messages/all", queryParams, await _helper.GetAccessTokenAsync());

            if (response.IsSuccessStatusCode)
            {
                return await _helper.DeserializeJsonAsync<IEnumerable<MessageDto>>(response);
            }
            else
            {
                return new List<MessageDto>();
            }
        }
    }
}

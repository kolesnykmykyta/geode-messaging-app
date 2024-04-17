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
        private readonly ILocalStorageService _localStorage;
        private readonly IServicesHelper _helper;

        public MessagesService(IHttpClientWrapper httpClient, ILocalStorageService localStorage, IServicesHelper helper)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _helper = helper;
        }

        public async Task<IEnumerable<ChatMessageDto>> GetAllChatMessages(int chatId)
        {
            string? accessToken = await _localStorage.GetItemAsStringAsync("BearerToken");
            HttpResponseMessage response = await _httpClient.GetAsync($"messages/chat/{chatId}", null, accessToken);

            return await _helper.DeserializeJsonAsync<IEnumerable<ChatMessageDto>>(response);
        }

        public async Task<IEnumerable<MessageDto>> GetAllUserMessagesAsync(FilterDto? filter)
        {
            Dictionary<string, string>? queryParams = filter == null ? null : _helper.CreateDictionaryFromObject(filter);
            string? accessToken = await _localStorage.GetItemAsStringAsync("BearerToken");
            HttpResponseMessage response = await _httpClient.GetAsync("messages/all", queryParams, accessToken);

            return await _helper.DeserializeJsonAsync<IEnumerable<MessageDto>>(response);
        }
    }
}

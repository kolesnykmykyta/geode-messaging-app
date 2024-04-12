using Application.Dtos;
using Application.Utils.HttpClientWrapper;
using Blazored.LocalStorage;
using Geode.Maui.Services.Interfaces;
using System.Reflection;
using System.Text.Json;

namespace Geode.Maui.Services
{
    internal class ChatsService : IChatsService
    {
        private readonly IHttpClientWrapper _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly IServicesHelper _helper;

        public ChatsService(IHttpClientWrapper httpClient, ILocalStorageService localStorage, IServicesHelper helper)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _helper = helper;
        }

        public async Task<bool> CreateNewChat(string chatName)
        {
            ChatDto newChat = new ChatDto() { Name = chatName };
            string? accessToken = await _localStorage.GetItemAsStringAsync("BearerToken");
            HttpResponseMessage response = await _httpClient.PostAsync("chat", newChat, accessToken);

            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<ChatDto>> GetAllUserChatsAsync(FilterDto? filter)
        {
            Dictionary<string, string>? queryParams = filter == null ? null : _helper.CreateDictionaryFromObject(filter);
            string? accessToken = await _localStorage.GetItemAsStringAsync("BearerToken");
            HttpResponseMessage response = await _httpClient.GetAsync("chat/all", queryParams, accessToken);

            return await _helper.DeserializeJsonAsync<IEnumerable<ChatDto>>(response);
        }
    }
}

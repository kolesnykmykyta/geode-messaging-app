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
        private readonly IServicesHelper _helper;

        public ChatsService(IHttpClientWrapper httpClient, IServicesHelper helper)
        {
            _httpClient = httpClient;
            _helper = helper;
        }

        public async Task<bool> CreateNewChat(string chatName)
        {
            ChatDto newChat = new ChatDto() { Name = chatName };
            HttpResponseMessage response = await _httpClient.PostAsync("chat", newChat, await _helper.GetAccessTokenAsync());

            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<ChatDto>> GetAllUserChatsAsync(FilterDto? filter)
        {
            Dictionary<string, string>? queryParams = filter == null ? null : _helper.CreateDictionaryFromObject(filter);
            HttpResponseMessage response = await _httpClient.GetAsync("chat/all", queryParams, await _helper.GetAccessTokenAsync());

            if (response.IsSuccessStatusCode)
            {
                return await _helper.DeserializeJsonAsync<IEnumerable<ChatDto>>(response);
            }
            else
            {
                return new List<ChatDto>();
            }
        }

        public async Task<bool> JoinChatAsync(string? chatId)
        {
            int intId;
            if (int.TryParse(chatId, out intId))
            {
                HttpResponseMessage response = await _httpClient.PostAsync($"chat/join/{chatId}", null, await _helper.GetAccessTokenAsync());
                return response.IsSuccessStatusCode;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> LeaveChatAsync(string? chatId)
        {
            int intId;
            if (int.TryParse(chatId, out intId))
            {
                HttpResponseMessage response = await _httpClient.PostAsync($"chat/leave/{chatId}", null, await _helper.GetAccessTokenAsync());
                return response.IsSuccessStatusCode;
            }
            else
            {
                return false;
            }
        }

        public async Task UpdateChatAsync(ChatDto dto)
        {
            HttpResponseMessage response = await _httpClient.PutAsync($"chat", dto, await _helper.GetAccessTokenAsync());
        }

        public async Task DeleteChatAsync(int chatId)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"chat/delete/{chatId}", await _helper.GetAccessTokenAsync());
        }
    }
}

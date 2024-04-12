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

        public ChatsService(IHttpClientWrapper httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public async Task<IEnumerable<ChatDto>> GetAllUserChatsAsync(FilterDto? filter)
        {
            Dictionary<string, string>? queryParams = filter == null ? null : CreateDictionaryFromObject(filter);
            string? accessToken = await _localStorage.GetItemAsStringAsync("BearerToken");
            HttpResponseMessage response = await _httpClient.GetAsync("chat/all", queryParams, accessToken);

            if (response.IsSuccessStatusCode)
            {
                string jsonResponseString = await response.Content.ReadAsStringAsync();
                List<ChatDto>? responseBody = JsonSerializer.Deserialize<List<ChatDto>>(jsonResponseString);
                return responseBody;
            }
            else
            {
                return null;
            }
        }

        private Dictionary<string, string> CreateDictionaryFromObject(object obj)
        {
            Type type = obj.GetType();
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var output = new Dictionary<string, string>();

            foreach (var prop in properties)
            {
                var value = prop.GetValue(obj);
                if (value != null)
                {
                    output.Add(prop.Name, value.ToString());
                }
            }

            return output;
        }
    }
}

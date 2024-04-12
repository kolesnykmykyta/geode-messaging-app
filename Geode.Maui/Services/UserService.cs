using Application.Dtos;
using Application.Utils.HttpClientWrapper;
using Auth.Dtos;
using Blazored.LocalStorage;
using Geode.Maui.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Geode.Maui.Services
{
    internal class UserService : IUsersService
    {
        private readonly IHttpClientWrapper _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly IServicesHelper _helper;

        public UserService(IHttpClientWrapper httpClient, ILocalStorageService localStorage, IServicesHelper helper)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _helper = helper;
        }

        public async Task<IEnumerable<UserInfoDto>> GetUserListAsync(FilterDto? filter)
        {
            Dictionary<string, string>? queryParams = filter == null ? null : _helper.CreateDictionaryFromObject(filter);
            string? accessToken = await _localStorage.GetItemAsStringAsync("BearerToken");
            HttpResponseMessage response = await _httpClient.GetAsync("user/all", queryParams, accessToken);

            return await _helper.DeserializeJsonAsync<IEnumerable<UserInfoDto>>(response);
        }

        private Dictionary<string,string> CreateDictionaryFromObject(object obj)
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
